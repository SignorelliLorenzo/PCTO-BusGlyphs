import 'dart:convert';
import 'dart:io';
import 'dart:async';
import 'package:app/models/mex.dart';
import 'package:path/path.dart';
import 'package:image/image.dart' as I;
import 'package:app/screens/bus_screen.dart';
import 'package:app/screens/home_screen.dart';
import 'package:app/screens/info_screen.dart';
import 'package:app/screens/map_screen.dart';
import 'package:flutter/material.dart';
import 'package:camerawesome/camerapreview.dart';
import 'package:camera/camera.dart';
import 'package:image_picker/image_picker.dart';
import 'package:web_socket_channel/io.dart';
import '../apis/connection.dart';
import '../main.dart';
import 'package:path_provider/path_provider.dart';

class CameraScreen extends StatefulWidget {
  /// Default Constructor
  const CameraScreen({Key? key}) : super(key: key);

  @override
  State<CameraScreen> createState() => _CameraScreenState();
}

class _CameraScreenState extends State<CameraScreen> {
  late CameraController controller;

  static List<int>? convertYUV420toImageColor(CameraImage image) {
    try {
      final int width = image.width;
      final int height = image.height;
      final int uvRowStride = image.planes[1].bytesPerRow;
      final int? uvPixelStride = image.planes[1].bytesPerPixel;

      // imgLib -> Image package from https://pub.dartlang.org/packages/image
      var img = I.Image(width, height); // Create Image buffer

      // Fill image buffer with plane[0] from YUV420_888
      for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
          final int uvIndex =
              uvPixelStride! * (x / 2).floor() + uvRowStride * (y / 2).floor();
          final int index = y * width + x;

          final yp = image.planes[0].bytes[index];
          final up = image.planes[1].bytes[uvIndex];
          final vp = image.planes[2].bytes[uvIndex];
          // Calculate pixel color
          int r = (yp + vp * 1436 / 1024 - 179).round().clamp(0, 255);
          int g = (yp - up * 46549 / 131072 + 44 - vp * 93604 / 131072 + 91)
              .round()
              .clamp(0, 255);
          int b = (yp + up * 1814 / 1024 - 227).round().clamp(0, 255);
          // color: 0x FF  FF  FF  FF
          //           A   B   G   R
          img.data[index] = (0xFF << 24) | (b << 16) | (g << 8) | r;
        }
      }

      I.PngEncoder pngEncoder = I.PngEncoder(level: 0, filter: 0);
      List<int> png = pngEncoder.encodeImage(img);
      return png;
    } catch (e) {}
    return null;
  }

  void getB64(CameraImage image) {
    String base64;
    var tempimg = convertYUV420toImageColor(image);
    base64 = base64Encode(tempimg!);
    print(base64);
    return;
  }

  late Future<void> _initializeControllerFuture;

  // void _initFrames() async {
  //   _initializeControllerFuture = controller.initialize();

  //   // XFile photo = await controller.takePicture();
  //   // List<int> photoAsBytes = await photo.readAsBytes();
  //   // String photoAsBase64 = base64Encode(photoAsBytes);
  //   // controller.startImageStream((CameraImage image) async {
  //   //   if (isDetecting) return;
  //   //   isDetecting = true;
  //   //   try {
  //   //     //   timer =
  //   //     //       Timer.periodic(Duration(seconds: 5), (Timer t) => getB64(image));
  //   //     // done = true;
  //   //     //GallerySaver.saveImage();

  //   //   } catch (e) {
  //   //     throw Exception("Error while detecting or sending frame");
  //   //   } finally {
  //   //     isDetecting = true;
  //   //   }
  //   //});
  // }

  @override
  void initState() {
    super.initState();
    controller = CameraController(cameras[0], ResolutionPreset.medium);
    _initializeControllerFuture = controller.initialize();
  }

  @override
  void dispose() {
    controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (!controller.value.isInitialized) {
      return Container();
    }
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          "BusGlyphs",
          style: TextStyle(
              fontStyle: FontStyle.normal,
              fontWeight: FontWeight.w400,
              fontSize: 28),
        ),
        leading: const Icon(Icons.bus_alert_sharp),
      ),
      body: FutureBuilder<void>(
        future: _initializeControllerFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.done) {
            // If the Future is complete, display the preview.
            return ClipRect(
                child: OverflowBox(
              alignment: Alignment.center,
              child: FittedBox(
                  fit: BoxFit.fitWidth,
                  child: Container(
                      clipBehavior: Clip.hardEdge,
                      decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(19.0),
                          border: Border.all(color: Colors.black, width: 3.5)),
                      width: 350,
                      height: 850 / controller.value.aspectRatio,
                      child: AspectRatio(
                        aspectRatio: controller.value.aspectRatio,
                        child: CameraPreview(controller),
                      ))),
            ));
          } else {
            // Otherwise, display a loading indicator.
            return Center(child: CircularProgressIndicator());
          }
        },
      ),
      floatingActionButton: FloatingActionButton(
        // Provide an onPressed callback.
        onPressed: () async {
          // Take the Picture in a try / catch block. If anything goes wrong,
          // catch the error.
          try {
            // Ensure that the camera is initialized.
            await _initializeControllerFuture;
            // Attempt to take a picture and then get the location
            // where the image file is saved.
            final path = join(
              // Store the picture in the temp directory.
              // Find the temp directory using the `path_provider` plugin.
              (await getTemporaryDirectory()).path,
              '${DateTime.now()}.png',
            );
            XFile image = await controller.takePicture();
            image.saveTo(path);
            final bytes = File(image.path).readAsBytesSync();
            String base64Image = base64Encode(bytes);

            ///->>sendImg(RequestImg(base64Image));
            print("img_pan : $base64Image");
          } catch (e) {
            // If an error occurs, log the error to the console.
            print(e);
          }
        },
        child: const Icon(Icons.camera_alt),
      ),
      // floatingActionButton: FloatingActionButton(
      //   //Floating action button on Scaffold
      //   onPressed: () {
      //     //code to execute on button press
      //   },
      //   child: const Icon(
      //     Icons.photo_camera,
      //     size: 30,
      //     color: Colors.grey,
      //   ), //icon inside button
      // ),

      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      //floating action button position to center

      bottomNavigationBar: BottomAppBar(
        //bottom navigation bar on scaffold
        color: Colors.red,
        shape: const CircularNotchedRectangle(), //shape of notch
        notchMargin:
            5, //notche margin between floating button and bottom appbar
        child: Row(
          //children inside bottom appbar
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            IconButton(
              icon: const Icon(
                Icons.home,
                color: Colors.white,
                size: 30,
              ),
              onPressed: () {
                Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (context) => const HomeScreen()));

                //code to execute on button press
              },
            ),
            IconButton(
              icon: const Icon(
                Icons.bus_alert_rounded,
                color: Colors.white,
                size: 30,
              ),
              onPressed: () {
                Navigator.push(context,
                    MaterialPageRoute(builder: (context) => const BusScreen()));
              },
            ),
            IconButton(
              icon: const Icon(
                Icons.location_pin,
                color: Colors.white,
                size: 30,
              ),
              onPressed: () {
                Navigator.push(context,
                    MaterialPageRoute(builder: (context) => const MapScreen()));
              },
            ),
            IconButton(
              icon: const Icon(
                Icons.info,
                color: Colors.white,
                size: 30,
              ),
              onPressed: () {
                Navigator.push(
                    context,
                    MaterialPageRoute(
                        builder: (context) => const InfoScreen()));
              },
            ),
          ],
        ),
      ),
    );
  }
}
