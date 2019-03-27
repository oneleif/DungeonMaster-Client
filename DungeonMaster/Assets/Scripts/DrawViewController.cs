using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// REQUIRES A COLLIDER2D to function
// 1. Attach this to a read/write enabled sprite image
// 2. Set the drawing_layers  to use in the raycast
// 3. Attach a 2D collider (like a Box Collider 2D) to this sprite
// 4. Hold down left mouse to draw on this texture!
public class DrawViewController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    DrawViewModel drawViewModel;

    public delegate void Brush_Function(Vector2 worldPosition);
    public Brush_Function currentBrush;

    // MUST HAVE READ/WRITE enabled set in the file editor of Unity
    Image drawImage;
    Sprite drawSprite;
    Texture2D drawTexture;

    Vector2 previousDragPosition;

    Color[] resetColorsArray;
    Color resetColor;

    Color32[] currentColors;

    RectTransform rectTransform;

    void Awake() {
        currentBrush = PenBrush;

        rectTransform = GetComponent<RectTransform>();
        drawImage = GetComponent<Image>();

        drawViewModel = new DrawViewModel();
    }

    public void Initialize() {
        drawSprite = drawImage.sprite;
        drawTexture = drawSprite.texture;

        resetColor = new Color(0, 0, 0, 0);
        resetColorsArray = new Color[(int)drawSprite.rect.width * (int)drawSprite.rect.height];

        for (int x = 0; x < resetColorsArray.Length; x++)
            resetColorsArray[x] = resetColor;
    }

    void Update() {
        KeyboardInput();
    }

    void KeyboardInput() {
#if UNITY_EDITOR
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            && Input.GetKeyDown(KeyCode.Z)) {
            Debug.Log("x");
            if (drawViewModel.CanUndo()) {
                Debug.Log("y");
                currentColors = drawViewModel.Undo(drawTexture.GetPixels32());
                ApplyMarkedPixelChanges();
            }
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            && Input.GetKeyDown(KeyCode.Y)) {
            if (drawViewModel.CanRedo()) {
                currentColors = drawViewModel.Redo(drawTexture.GetPixels32());
                ApplyMarkedPixelChanges();
            }
        }
#else
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
#endif
    }


    // Pass in a point in WORLD coordinates
    // Changes the surrounding pixels of the world_point to the pen_colour
    public void PenBrush(Vector2 pixelPosition) {
        //Make sure our variable for pixel array is updated in this frame
        currentColors = drawTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero) {
            // If this is the first time we've ever dragged on this image, simply colour the pixels at our mouse position
            MarkPixelsToColour(pixelPosition, drawViewModel.penWidth);
        } else {
            // Color in a line from where we were on the last update call
            ColorBetween(previousDragPosition, pixelPosition, drawViewModel.penWidth);
        }
        ApplyMarkedPixelChanges();

        //Debug.Log("Dimensions: " + pixelWidth + "," + pixelHeight + ". Units to pixels: " + unitsToPixels + ". Pixel pos: " + pixel_pos);
        previousDragPosition = pixelPosition;
    }

    public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness) {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;
        //int extra_radius = Mathf.Min(0, pen_thickness - 2);

        for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++) {
            // Check if the X wraps around the image, so we don't draw pixels on the other side of the image
            if (x >= (int)drawSprite.rect.width || x < 0)
                continue;

            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++) {
                MarkPixelToChange(x, y);
            }
        }
    }

    // Set the colour of pixels in a straight line from start_point all the way to end_point, to ensure everything inbetween is coloured
    public void ColorBetween(Vector2 start_point, Vector2 end_point, int width) {
        // Get the distance from start to finish
        float distance = Vector2.Distance(start_point, end_point);
        Vector2 direction = (start_point - end_point).normalized;

        Vector2 cur_position = start_point;

        // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps) {
            cur_position = Vector2.Lerp(start_point, end_point, lerp);
            MarkPixelsToColour(cur_position, width);
        }
    }


    public void MarkPixelToChange(int x, int y) {
        // Need to transform x and y coordinates to flat coordinates of array
        int array_pos = y * (int)drawSprite.rect.width + x;

        // Check if this is a valid position
        if (array_pos > currentColors.Length || array_pos < 0)
            return;

        currentColors[array_pos] = drawViewModel.penColor;
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

    // Changes every pixel to be the reset colour
    public void ResetCanvas() {
        drawTexture.SetPixels(resetColorsArray);
        drawTexture.Apply();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        drawViewModel.AddUndo(drawTexture.GetPixels32());
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 localCursor = Vector2.zero;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out localCursor)) {
            return;
        }

        //Check if the cursor is in the bounds of the image
        //Pivot MUST be 0,0
        if (Mathf.Abs(localCursor.x) < drawImage.sprite.rect.width && Mathf.Abs(localCursor.y) < drawImage.sprite.rect.height) {
            float rectToPixelScale = drawImage.sprite.rect.width / rectTransform.rect.width;
            localCursor = new Vector2(localCursor.x * rectToPixelScale, localCursor.y * rectToPixelScale);
            currentBrush(localCursor);
        }

        previousDragPosition = localCursor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        previousDragPosition = Vector2.zero;
    }

    public void SetDrawColor(Color color) {
        drawViewModel.SetDrawColour(color);
    }

    public Color GetDrawColor() {
        return drawViewModel.penColor;
    }

    public void SetDrawLineWidth(int width) {
        drawViewModel.SetLineWidth(width);
    }

    public int GetDrawLineWidth() {
        return drawViewModel.penWidth;
    }

    public void SetDrawTransparency(float transparency) {
        drawViewModel.SetAlpha(transparency);
    }

    public float GetDrawTransparency() {
        return drawViewModel.transparency;
    }
}