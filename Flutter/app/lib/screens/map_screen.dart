import 'package:app/screens/camera_screen.dart';
import 'package:app/screens/home_screen.dart';
import 'package:app/screens/info_screen.dart';
import 'package:flutter/material.dart';

class MapScreen extends StatefulWidget {
  /// Default Constructor
  const MapScreen({Key? key}) : super(key: key);

  @override
  State<MapScreen> createState() => _MapScreenState();
}

void reloadImg() {
  //Update img on new img
}

class _MapScreenState extends State<MapScreen> {
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
                    borderRadius: BorderRadius.circular(15.0),
                    border: Border.all(color: Colors.black, width: 5.0)),
                width: 450,
                height: 400,
                child: Container(
                  color: Colors.grey,
                ))),
      )),
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
              onPressed: () {},
            ),
            IconButton(
              icon: const Icon(
                Icons.location_pin,
                color: Colors.grey,
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
