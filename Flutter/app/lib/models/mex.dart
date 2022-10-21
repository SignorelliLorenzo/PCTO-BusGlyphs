import 'bus.dart';
import 'fermata.dart';

class Base {
  late bool status;
  late List<String> error;
}

class RequestImg {
  String img;
  RequestImg(this.img);
}

class ResponseImg implements Base {
  late bool status;
  late List<String> error;
  late Fermata fermata;
  late List<Fermata> arriviprob;
}

class RequestBus {
  late Fermata partenza;
  late Fermata arrivo;
}

class ResponseBus implements Base {
  late bool status;
  late List<String> error;
  late Fermata attuale;
  late List<Bus> buses;
}

class RequestNearBus {
  late Fermata attuale;
  late List<Bus> buses;
}

class ResponseNearBus implements Base {
  late bool status;
  late List<String> error;
  late Bus bus;
}

class RequestPosition {
  late Bus bus;
}

class ResponsePosition implements Base {
  late bool status;
  late List<String> error;
  late String img;
}
