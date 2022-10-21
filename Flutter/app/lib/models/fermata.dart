class Fermata {
  List<String> Ids = <String>[];
  late String id;
  Fermata(String id, double x, double y) {
    if (Ids.contains(id) || id.isEmpty) {
      throw const FormatException("Ids must be unique and not null");
    }
    id = id;
    GPS coord = GPS();
    coord.x = x;
    coord.y = y;
    Ids.add(id);
  }
}

class GPS {
  late double x;
  late double y;
}
