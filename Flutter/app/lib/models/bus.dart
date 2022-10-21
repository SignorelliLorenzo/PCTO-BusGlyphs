import 'percorso.dart';

class Bus {
  late String id;
  Percorso percorso;
  late bool stato;
  String nome;
  List<String> Ids = <String>[];

  Bus(String id, this.percorso, this.nome) {
    if (Ids.contains(id) || id.isEmpty) {
      throw Exception("Id must be unique and not null");
    }
    id = id;
    Ids.add(id);
  }
}
