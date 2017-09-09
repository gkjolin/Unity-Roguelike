using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AKSaigyouji.Modules.MapGeneration
{
    public sealed class SpritePostProcessor : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (IsInCorrectFolder(assetPath))
            {
                TextureImporter importer = (TextureImporter)assetImporter;
                importer.alphaIsTransparency = true;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.filterMode = FilterMode.Point;
                importer.isReadable = true;

                importer.spritePixelsPerUnit = assetPath.Contains("/Items/") ? 32 : 54;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.textureType = TextureImporterType.Sprite;
                var platformImporter = new TextureImporterPlatformSettings();
                platformImporter.format = TextureImporterFormat.RGBA32;
                platformImporter.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SetPlatformTextureSettings(platformImporter);
            }
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            if (IsInCorrectFolder(assetPath))
            {
                var pixels = texture.GetPixels32();
                int width = texture.width;
                int height = texture.height;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var color = pixels[y * width + x];
                        if (color.r == 255 && color.b == 255 && color.g == 0)
                        {
                            color.a = 0;
                            pixels[y * width + x] = color;
                        }
                    }
                }
                texture.SetPixels32(pixels);
                texture.Apply();
            }
        }

        bool IsInCorrectFolder(string path)
        {
            return assetPath.Contains("ColorToAlpha");
        }
    } 
}