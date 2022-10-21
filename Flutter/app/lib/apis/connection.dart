import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:web_socket_channel/io.dart';
import 'package:web_socket_channel/status.dart' as status;
import 'package:web_socket_channel/web_socket_channel.dart';

import '../models/bus.dart';
import '../models/mex.dart';

//---ConnectionStrings---//
String channel_ServerImg = "";
String channel_ServerBuses = "";
String channel_ServerPosition = "";
String channel_ServerNearBus = "";

//---ServerImg---//
void sendReqImg(RequestImg mex, WebSocketChannel channel) {
  channel.sink.add(jsonEncode(mex));
}

ResponseImg? getStops(WebSocketChannel channel) {
  ResponseImg? obj;
  channel.stream.listen((stops) {
    obj = jsonDecode(stops);
    channel.sink.close(status.goingAway);
  });
  if (obj!.status) {
    return obj;
  }
  return null;
}

//---ServerPosition---//
void sendReqPos(RequestPosition mex, WebSocketChannel channel) {
  channel.sink.add(jsonEncode(mex));
}

ResponsePosition? getMap(WebSocketChannel channel) {
  ResponsePosition? obj;
  channel.stream.listen((position) {
    obj = jsonDecode(position);
    channel.sink.close(status.goingAway);
  });
  if (obj!.status) {
    return obj;
  }
  return null;
}

//---ServerNearBus---//
void sendReqNearBus(RequestNearBus mex, WebSocketChannel channel) {
  channel.sink.add(jsonEncode(mex));
}

ResponseNearBus? getNearBuse(WebSocketChannel channel) {
  ResponseNearBus? obj;
  channel.stream.listen((nearBusses) {
    obj = jsonDecode(nearBusses);
    channel.sink.close(status.goingAway);
  });
  if (obj!.status) {
    return obj;
  }
  return null;
}

//---ServerBuses---//
void sendReqBus(RequestBus mex, WebSocketChannel channel) {
  channel.sink.add(jsonEncode(mex));
}

ResponseBus? getBuses(WebSocketChannel channel) {
  ResponseBus? obj;
  channel.stream.listen((busses) {
    obj = jsonDecode(busses);
    channel.sink.close(status.goingAway);
  });
  if (obj!.status) {
    return obj;
  }
  return null;
}
