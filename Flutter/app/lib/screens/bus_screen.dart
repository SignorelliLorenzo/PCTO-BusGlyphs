import 'package:app/apis/connection.dart';
import 'package:app/screens/camera_screen.dart';
import 'package:app/screens/home_screen.dart';
import 'package:app/screens/info_screen.dart';
import 'package:app/screens/map_screen.dart';
import 'package:flutter/material.dart';

class BusScreen extends StatefulWidget {
  /// Default Constructor
  const BusScreen({Key? key}) : super(key: key);

  @override
  State<BusScreen> createState() => _BusScreenState();
}

class _BusScreenState extends State<BusScreen> {
  String? startvar;
  final _dropdownFormKey = GlobalKey<FormState>();
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
      body: Form(
          key: _dropdownFormKey,
          child: Column(mainAxisAlignment: MainAxisAlignment.start, children: [
            Padding(
                padding: const EdgeInsets.all(10.0),
                child: DropdownButtonFormField(
                  decoration: InputDecoration(
                    enabledBorder: OutlineInputBorder(
                      borderSide: BorderSide(color: Colors.black, width: 2),
                      borderRadius: BorderRadius.circular(20),
                    ),
                    border: OutlineInputBorder(
                      borderSide: BorderSide(color: Colors.black, width: 2),
                      borderRadius: BorderRadius.circular(20),
                    ),
                    filled: true,
                    fillColor: Colors.grey,
                  ),
                  validator: (value) =>
                      value == null ? "Select a country" : null,
                  dropdownColor: Colors.grey,
                  value: startvar,
                  onChanged: (String? newValue) {
                    setState(() {
                      startvar = newValue!;
                    });
                    //send
                  },
                  items: null, //getStops
                ))
          ])),
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
                size: 30,
                color: Colors.white,
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
                color: Colors.grey,
                size: 30,
              ),
              onPressed: () {},
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
