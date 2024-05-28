using UnityEngine;
using UnityEngine.UI;

public class PixelArtConverter : MonoBehaviour
{
    [SerializeField] private Texture2D originImage;
    [SerializeField] private int pixelSize = 16;

    private void Start()
    {
        Texture2D pixelArt = ConvertToPixelArt(originImage, pixelSize);
        ApplyTexture(pixelArt);
    }

    private Texture2D ConvertToPixelArt(Texture2D _originImage, int _pixelSize)
    {
        int width = _originImage.width;
        int height = _originImage.height;

        // 다운샘플링
        Texture2D smallTexture = new Texture2D(width / pixelSize, height / pixelSize);
        for (int y = 0; y < smallTexture.height; ++y)
        {
            for (int x = 0; x < smallTexture.width; ++x)
            {
                Color averageColor = GetAverageColor(_originImage, x * pixelSize, y * pixelSize, pixelSize);
                smallTexture.SetPixel(x, y, averageColor);
            }
        }
        smallTexture.Apply();

        // 업스케일링
        Texture2D pixelArt = new Texture2D(width, height);
        for (int y = 0; y < pixelArt.height; ++y)
        {
            for (int x = 0; x < pixelArt.width; ++x)
            {
                Color color = smallTexture.GetPixel(x / pixelSize, y / pixelSize);
                pixelArt.SetPixel(x, y, color);
            }
        }
        pixelArt.Apply();

        return pixelArt;

    }

    private Color GetAverageColor(Texture2D _texture, int _startX, int _startY, int _size)
    {
        Color sum = Color.black;
        int count = 0;

        for (int y = _startY; y < _startY + _size && y < _texture.height; ++y)
        {
            for (int x = _startX; x < _startX + _size && x < _texture.width; ++x)
            {
                sum += _texture.GetPixel(x, y);
                count++;
            }
        }
        return sum / count;
    }

    private void ApplyTexture(Texture2D _texture)
    {
        Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<Image>().sprite = sprite;

    }
}
