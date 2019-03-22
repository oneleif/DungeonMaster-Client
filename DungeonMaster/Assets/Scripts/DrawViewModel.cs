using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawViewModel {
    // PEN COLOUR
    public Color penColor = Color.red;
    // PEN WIDTH (actually, it's a radius, in pixels)
    public int penWidth = 1000;


    public Stack<Color32[]> undos;
    public Stack<Color32[]> redos;

    public DrawViewModel() {
        undos = new Stack<Color32[]>();
        redos = new Stack<Color32[]>();
    }

    public void SetMarkerColour(Color new_color) {
        penColor = new_color;
    }

    public void SetMarkerWidth(int new_width) {
        penWidth = new_width;
    }

    public void SetAlpha(float amount) {
        Color c = penColor;
        c.a = amount;
        penColor = c;
    }

    public void AddUndo(Color32[] undo) {
        Debug.Log("adding undo");
        undos.Push(undo);
        redos.Clear();
    }

    public Color32[] Undo(Color32[] newState) {
        Debug.Log("undo");

        Color32[] undoToGet = undos.Pop();
        redos.Push(newState);
        return undoToGet;
    }

    public bool CanUndo() {
        return undos.Count > 0;
    }

    public Color32[] Redo(Color32[] newState) {
        Debug.Log("redo");

        Color32[] redoToGet = redos.Pop();
        undos.Push(newState);
        return redoToGet;
    }

    public bool CanRedo() {
        return redos.Count > 0;
    }

}