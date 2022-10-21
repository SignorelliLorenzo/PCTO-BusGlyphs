import 'dart:convert';
import 'dart:io';
import 'dart:async';
import 'package:app/apis/connection.dart';
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
import 'package:web_socket_channel/web_socket_channel.dart';
import '../main.dart';
import 'package:path_provider/path_provider.dart';

import 'dart:io';
import 'package:camera/camera.dart';
import 'package:flutter/material.dart';

import '../models/mex.dart';

class CameraScreen extends StatefulWidget {
  /// Default Constructor
  const CameraScreen({Key? key}) : super(key: key);

  @override
  State<CameraScreen> createState() => _CameraScreenState();
}

class _CameraScreenState extends State<CameraScreen> {
  List<CameraDescription>? cameras; //list out the camera available
  CameraController? controller; //controller for camera
  late XFile image; //for captured image
  ResponseImg? obj;
  //final channel = WebSocketChannel.connect(Uri.parse(channel_ServerImg));
  @override
  void initState() {
    loadCamera();
    super.initState();
  }

  loadCamera() async {
    cameras = await availableCameras();
    if (cameras != null) {
      controller = CameraController(cameras![0], ResolutionPreset.medium);
      //cameras[0] = first camera, change to 1 to another camera

      controller!.initialize().then((_) {
        if (!mounted) {
          return;
        }
        setState(() {});
      });
    } else {
      print("NO any camera found");
    }
  }

  @override
  Widget build(BuildContext context) {
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
      body: ClipRect(
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
                    height: 520,
                    child: controller == null
                        ? const Center(child: Text("Loading Camera..."))
                        : !controller!.value.isInitialized
                            ? const Center(
                                child: CircularProgressIndicator(),
                              )
                            : CameraPreview(controller!)),
              ))),
      floatingActionButton: FloatingActionButton(
        //image capture button
        onPressed: () async {
          try {
            if (controller != null) {
              //check if contrller is not null
              if (controller!.value.isInitialized) {
                //check if controller is initialized
                String path = join(
                  (await getTemporaryDirectory()).path,
                  '${DateTime.now()}.png',
                );
                image = await controller!.takePicture();
                setState(() {
                  //update UI
                });
                final bytes = File(image.path).readAsBytesSync();
                String base64Image = base64Encode(bytes);

                // sendReqImg(RequestImg(base64Image), channel);
                // obj = getStops(channel);
                // if (obj != null) {
                //   setState(() {
                //     Navigator.push(
                //         context,
                //         MaterialPageRoute(
                //             builder: (context) => const BusScreen()));
                //   });
                // }
              }
            }
          } catch (e) {
            print(e); //show error
          }
        },
        child: const Icon(Icons.camera_alt),
      ),
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
