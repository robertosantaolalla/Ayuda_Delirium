using UnityEngine;
using System;

public class ImagePickerCroperHandler : MonoBehaviour {

    public void PickImage(Action<bool> success, Action<string> imageBytes) {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => { // variable path con el string correcto
            if (path != null) {
                Texture2D texture = NativeGallery.DeCompress(NativeGallery.LoadImageAtPath(path, 5000)); // Cargar imágen de path con 5000mb de tamaño máximo

                string last3chars = path.Substring(path.Length - 3).ToLower();
                bool validFormat = (last3chars == "jpg" || last3chars == "png" || last3chars == "gif" || last3chars == "bmp" || last3chars == "peg");

                if (texture == null || !validFormat) {
                    success(false); // Could not load texture from path  or invalid format
                    return;
                }
                ImageCropper.Instance.Show(texture, (bool result, Texture originalImage, Texture2D croppedImage) => {
                    if (result) { // If screenshot was cropped successfully
                        Texture2D textureResult = NativeGallery.DeCompress(croppedImage);
                        byte[] texByte = textureResult.EncodeToPNG(); // save texture in png format to keep the transparencies
                        string base64Tex = Convert.ToBase64String(texByte); // convert byte array to base64 string
                        success(true);
                        imageBytes(base64Tex);
                    }
                    else {
                        success(false); // Error cortando la imágen
                    }
                        Destroy(texture);
                },
                settings: new ImageCropper.Settings() {
                    ovalSelection = false,
                    autoZoomEnabled = true,
                    imageBackground = Color.clear, // transparent background
                    selectionMinAspectRatio = 1f,
                    selectionMaxAspectRatio = 1f

                },
                croppedImageResizePolicy: (ref int width, ref int height) => {});
            }
            else {
                Debug.Log("Path null");
                success(false); // PATH NULL
            }
        }, "Seleccione una imágen de su galería", "image/png"); // Permission result: permission

    }


}