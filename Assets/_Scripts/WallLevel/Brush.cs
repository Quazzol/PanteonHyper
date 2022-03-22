using System;
using Misc;
using Pooling;
using UnityEngine;

namespace WallLevel
{
    public class Brush : PooledMonoBehaviour
    {
        public event Action<float> CompletePercent;
        public override int InitialPoolSize => 1;
        public bool IsActive { get; private set; }

        [SerializeField]
        private Color _brushColor = Color.red;
        [SerializeField]
        private int _brushSize = 32;

        private Transform _transform;
        private Camera _camera;
        private WallData _wallData = null;

        private void Start()
        {
            _camera = Camera.main;
            _transform = transform;
        }

        private void Update()
        {
            if (!IsActive)
                return;

            PaintWall();
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        private void PaintWall()
        {
            var coord = ReadInput();
            if (coord == null)
                return;

            var pixelUV = (Vector2)coord;
            pixelUV.x = pixelUV.x * _wallData.CreatedTexture.width - _brushSize / 2;
            pixelUV.y = pixelUV.y * _wallData.CreatedTexture.height - _brushSize / 2;

            int width = GetWidth(pixelUV.x);
            int height = GetHeigth(pixelUV.y);

            pixelUV.x = Mathf.Clamp(pixelUV.x, 0, _wallData.CreatedTexture.width - 1);
            pixelUV.y = Mathf.Clamp(pixelUV.y, 0, _wallData.CreatedTexture.height - 1);

            var colors = new Color[width * height];
            for (var i = 0; i < width * height; i++)
            {
                colors[i] = _brushColor;
            }

            _wallData.CreatedTexture.SetPixels((int)pixelUV.x, (int)pixelUV.y, width, height, colors, 0);
            _wallData.CreatedTexture.Apply();

            var wallColors = _wallData.CreatedTexture.GetPixels(0, 0, _wallData.CreatedTexture.width, _wallData.CreatedTexture.height);

            float paintedPixels = 0;
            foreach (var color in wallColors)
            {
                if (color.Equals(_brushColor))
                    paintedPixels++;
            }

            CompletePercent?.Invoke(paintedPixels / (_wallData.CreatedTexture.width * _wallData.CreatedTexture.height));
        }

        private Vector2? ReadInput()
        {
            if (!Input.GetMouseButton(0))
                return null;

            if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 20f))
                return null;

            if (!hit.transform.TryGetComponent<Wall>(out var wall))
                return null;

            CreateWallData(wall);

            return hit.textureCoord;
        }

        private void CreateWallData(Wall wall)
        {
            if (_wallData != null)
                return;

            var renderer = wall.GetComponent<Renderer>();
            var texture = Instantiate(renderer.material.mainTexture) as Texture2D;
            _wallData = new WallData(renderer, texture);
            renderer.material.mainTexture = texture;
        }

        private int GetWidth(float uvWidth)
        {
            var paintWidth = _brushSize;
            if (uvWidth + _brushSize >= _wallData.CreatedTexture.width)
            {
                paintWidth -= (int)(uvWidth + _brushSize - _wallData.CreatedTexture.width);
            }
            else if (uvWidth < 0)
            {
                paintWidth += (int)(uvWidth);
            }

            return paintWidth;
        }

        private int GetHeigth(float uvHeigth)
        {
            var paintHeigth = _brushSize;
            if (uvHeigth + _brushSize >= _wallData.CreatedTexture.height)
            {
                paintHeigth -= (int)(uvHeigth + _brushSize - _wallData.CreatedTexture.height);
            }
            else if (uvHeigth < 0)
            {
                paintHeigth += (int)(uvHeigth);
            }

            return paintHeigth;
        }
    }

    public class WallData
    {
        public Renderer Renderer { get; private set; }
        public Texture2D CreatedTexture { get; private set; }

        public WallData(Renderer renderer, Texture2D texture)
        {
            Renderer = renderer;
            CreatedTexture = texture;
        }

    }
}