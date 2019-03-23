using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]  // REQUIRES A COLLIDER2D to function
// 1. Attach this to a read/write enabled sprite image
// 2. Set the drawing_layers  to use in the raycast
// 3. Attach a 2D collider (like a Box Collider 2D) to this sprite
// 4. Hold down left mouse to draw on this texture!
public class DrawViewController : MonoBehaviour {
    DrawViewModel drawViewModel;

    public delegate void Brush_Function(Vector2 worldPosition);
    public Brush_Function currentBrush;

    public LayerMask drawLayerMask;

    // MUST HAVE READ/WRITE enabled set in the file editor of Unity
    Sprite drawSprite;
    Texture2D drawTexture;

    Vector2 previousDragPosition;

    Color[] resetColorsArray;
    Color resetColor;

    Color32[] currentColors;

    bool mouseWasHeldDown = false;
    bool isNotDrawingOnCurrentDrag = false;

    void Awake() {
        currentBrush = PenBrush;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        drawSprite = renderer.sprite;
        renderer.material.renderQueue = 3001;
        drawTexture = drawSprite.texture;

        drawViewModel = new DrawViewModel();

        resetColor = new Color(0, 0, 0, 0);
        resetColorsArray = new Color[(int)drawSprite.rect.width * (int)drawSprite.rect.height];

        for (int x = 0; x < resetColorsArray.Length; x++)
            resetColorsArray[x] = resetColor;

        ResetCanvas();
    }

    void Update() {
        MouseInput();
        KeyboardInput();
    }

    void MouseInput() {
        bool isMouseHeldDown = Input.GetMouseButton(0);
        if (isMouseHeldDown && !isNotDrawingOnCurrentDrag) {
            // Convert mouse coordinates to world coordinates
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the current mouse position overlaps our image
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);
            if (hit != null && hit.transform != null) {
                if (!mouseWasHeldDown) {
                    drawViewModel.AddUndo(drawTexture.GetPixels32());
                }
                // We're over the texture we're drawing on!
                // Use whatever function the current brush is
                currentBrush(mouseWorldPosition);
            } else {
                // We're not over our destination texture
                previousDragPosition = Vector2.zero;
                if (!mouseWasHeldDown) {
                    // This is a new drag where the user is left clicking off the canvas
                    // Ensure no drawing happens until a new drag is started
                    isNotDrawingOnCurrentDrag = true;
                }
            }
        }
        // Mouse is released
        else if (!isMouseHeldDown) {
            previousDragPosition = Vector2.zero;
            isNotDrawingOnCurrentDrag = false;
        }
        mouseWasHeldDown = isMouseHeldDown;
    }

    void KeyboardInput() {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            && Input.GetKeyDown(KeyCode.Z)) {
            if (Input.GetKey(KeyCode.LeftShift) && drawViewModel.CanRedo()) {
                currentColors = drawViewModel.Redo(drawTexture.GetPixels32());
                ApplyMarkedPixelChanges();
            } else if (drawViewModel.CanUndo()) {
                currentColors = drawViewModel.Undo(drawTexture.GetPixels32());
                ApplyMarkedPixelChanges();
            }
        }
    }

    // Pass in a point in WORLD coordinates
    // Changes the surrounding pixels of the world_point to the pen_colour
    public void PenBrush(Vector2 worldPoint) {
        // 1. Change world position to pixel coordinates
        Vector2 pixelPosition = WorldToPixelCoordinates(worldPoint);
        // 2. Make sure our variable for pixel array is updated in this frame
        currentColors = drawTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero) {
            // If this is the first time we've ever dragged on this image, simply colour the pixels at our mouse position
            MarkPixelsToColour(pixelPosition, drawViewModel.penWidth, drawViewModel.penColor);
        } else {
            // Color in a line from where we were on the last update call
            ColorBetween(previousDragPosition, pixelPosition, drawViewModel.penWidth, drawViewModel.penColor);
        }
        ApplyMarkedPixelChanges();

        //Debug.Log("Dimensions: " + pixelWidth + "," + pixelHeight + ". Units to pixels: " + unitsToPixels + ". Pixel pos: " + pixel_pos);
        previousDragPosition = pixelPosition;
    }

    public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness, Color color_of_pen) {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;
        //int extra_radius = Mathf.Min(0, pen_thickness - 2);

        for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++) {
            // Check if the X wraps around the image, so we don't draw pixels on the other side of the image
            if (x >= (int)drawSprite.rect.width || x < 0)
                continue;

            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++) {
                MarkPixelToChange(x, y, color_of_pen);
            }
        }
    }

    // Set the colour of pixels in a straight line from start_point all the way to end_point, to ensure everything inbetween is coloured
    public void ColorBetween(Vector2 start_point, Vector2 end_point, int width, Color color) {
        // Get the distance from start to finish
        float distance = Vector2.Distance(start_point, end_point);
        Vector2 direction = (start_point - end_point).normalized;

        Vector2 cur_position = start_point;

        // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps) {
            cur_position = Vector2.Lerp(start_point, end_point, lerp);
            MarkPixelsToColour(cur_position, width, color);
        }
    }


    public void MarkPixelToChange(int x, int y, Color color) {
        // Need to transform x and y coordinates to flat coordinates of array
        int array_pos = y * (int)drawSprite.rect.width + x;

        // Check if this is a valid position
        if (array_pos > currentColors.Length || array_pos < 0)
            return;

        currentColors[array_pos] = color;
    }
    public void ApplyMarkedPixelChanges() {
        drawTexture.SetPixels32(currentColors);
        drawTexture.Apply();
    }


    // Directly colours pixels. This method is slower than using MarkPixelsToColour then using ApplyMarkedPixelChanges
    // SetPixels32 is far faster than SetPixel
    // Colours both the center pixel, and a number of pixels around the center pixel based on pen_thickness (pen radius)
    public void ColorPixels(Vector2 center_pixel, int pen_thickness, Color color_of_pen) {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;
        //int extra_radius = Mathf.Min(0, pen_thickness - 2);

        for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++) {
            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++) {
                drawTexture.SetPixel(x, y, color_of_pen);
            }
        }

        drawTexture.Apply();
    }


    public Vector2 WorldToPixelCoordinates(Vector2 world_position) {
        // Change coordinates to local coordinates of this image
        Vector3 local_pos = transform.InverseTransformPoint(world_position);

        // Change these to coordinates of pixels
        float pixelWidth = drawSprite.rect.width;
        float pixelHeight = drawSprite.rect.height;
        float unitsToPixels = pixelWidth / drawSprite.bounds.size.x * transform.localScale.x;

        // Need to center our coordinates
        float centered_x = local_pos.x * unitsToPixels + pixelWidth / 2;
        float centered_y = local_pos.y * unitsToPixels + pixelHeight / 2;

        // Round current mouse position to nearest pixel
        Vector2 pixel_pos = new Vector2(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));

        return pixel_pos;
    }


    // Changes every pixel to be the reset colour
    public void ResetCanvas() {
        drawTexture.SetPixels(resetColorsArray);
        drawTexture.Apply();
    }
}