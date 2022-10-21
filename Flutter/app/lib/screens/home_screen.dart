import 'dart:io';

import 'package:app/screens/bus_screen.dart';
import 'package:app/screens/camera_screen.dart';
import 'package:app/screens/info_screen.dart';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:image/image.dart' as I;
import 'map_screen.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

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
      body: Container(
        alignment: Alignment.topRight,
        //child: Image.asset("assets/images/image.jpg")),
      ),
      floatingActionButton: FloatingActionButton(
        //Floating action button on Scaffold
        onPressed: () {
          Navigator.push(context,
              MaterialPageRoute(builder: (context) => const CameraScreen()));

          //code to execute on button press
        },
        child: const Icon(
          Icons.photo_camera,
          size: 30,
        ), //icon inside button
      ),

      floatingActionButtonLocation: FloatingActionButtonLocation.centerDocked,
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
                color: Colors.grey,
                size: 30,
              ),
              onPressed: () {},
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
