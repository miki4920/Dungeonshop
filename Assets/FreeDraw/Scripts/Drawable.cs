using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeDraw
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Drawable : MonoBehaviour
    {
        public Texture2D drawingTexture;
        public int PenRadius = 10;

        public delegate void Brush_Function(Vector2 world_position);
        public Brush_Function current_brush;
        public bool Reset_Canvas_On_Play = true;
        public Color Reset_Colour = new Color(0, 0, 0, 0);
        public static Drawable drawable;
        Sprite drawable_sprite;
        Texture2D drawable_texture;

        Vector2 previous_drag_position;
        Color[] clean_colours_array;
        Color transparent;
        Color32[] cur_colors;

        public void PenBrush(Vector2 world_point)
        {
            Vector2 pixel_pos = WorldToPixelCoordinates(world_point);

            cur_colors = drawable_texture.GetPixels32();

            if (previous_drag_position == Vector2.zero)
            {
                MarkPixelsToColour(pixel_pos, PenRadius, drawingTexture);
            }
            else
            {
                ColourBetween(previous_drag_position, pixel_pos, PenRadius, drawingTexture);
            }
            ApplyMarkedPixelChanges();

            previous_drag_position = pixel_pos;
        }

        public void SetPenBrush()
        {
            current_brush = PenBrush;
        }

        void Update()
        {
            bool mouse_held_down = Input.GetMouseButton(0);
            if (mouse_held_down)
            {
                Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                current_brush(mouse_world_position);
            }
            else if (!mouse_held_down)
            {
                previous_drag_position = Vector2.zero;
            }
        }

        public void ColourBetween(Vector2 start_point, Vector2 end_point, int width, Texture2D drawingTexture)
        {
            float distance = Vector2.Distance(start_point, end_point);
            Vector2 direction = (start_point - end_point).normalized;

            Vector2 cur_position = start_point;

            float lerp_steps = 1 / (distance / PenRadius);

            for (float lerp = 0; lerp <= 1; lerp += lerp_steps)
            {
                cur_position = Vector2.Lerp(start_point, end_point, lerp);
                MarkPixelsToColour(cur_position, width, drawingTexture);
            }
        }

        public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness, Texture2D drawingTexture)
        {
            int center_x = (int)center_pixel.x;
            int center_y = (int)center_pixel.y;

            for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++)
            {
                if (x >= (int)drawable_sprite.rect.width || x < 0)
                    continue;

                for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++)
                {
                    MarkPixelToChange(x, y, drawingTexture);
                }
            }
        }
        public void MarkPixelToChange(int x, int y, Texture2D drawingTexture)
        {
            int array_pos = y * (int)drawable_sprite.rect.width + x;

            if (array_pos >= cur_colors.Length || array_pos < 0)
                return;
            cur_colors[array_pos] = drawingTexture.GetPixel(x % drawingTexture.width, y % drawingTexture.height);
        }
        public void ApplyMarkedPixelChanges()
        {
            drawable_texture.SetPixels32(cur_colors);
            drawable_texture.Apply();
        }

        public Vector2 WorldToPixelCoordinates(Vector2 world_position)
        {
            Vector3 local_pos = transform.InverseTransformPoint(world_position);

            float pixelWidth = drawable_sprite.rect.width;
            float pixelHeight = drawable_sprite.rect.height;
            float unitsToPixels = pixelWidth / drawable_sprite.bounds.size.x * transform.localScale.x;

            float centered_x = local_pos.x * unitsToPixels + pixelWidth / 2;
            float centered_y = local_pos.y * unitsToPixels + pixelHeight / 2;

            Vector2 pixel_pos = new Vector2(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));

            return pixel_pos;
        }

        public void ResetCanvas()
        {
            drawable_texture.SetPixels(clean_colours_array);
            drawable_texture.Apply();
        }

        void Awake()
        {
            drawable = this;
            current_brush = PenBrush;

            drawable_sprite = this.GetComponent<SpriteRenderer>().sprite;
            drawable_texture = drawable_sprite.texture;

            clean_colours_array = new Color[(int)drawable_sprite.rect.width * (int)drawable_sprite.rect.height];
            for (int x = 0; x < clean_colours_array.Length; x++)
                clean_colours_array[x] = Reset_Colour;

            if (Reset_Canvas_On_Play)
                ResetCanvas();
        }
    }
}