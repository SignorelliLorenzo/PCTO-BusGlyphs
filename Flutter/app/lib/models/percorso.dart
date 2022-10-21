class Percorso {
  late String nome;
  List<int> elefermateandata = <int>[];
  List<int> elefermateritorno = <int>[];
  List<String> elenomi = <String>[];

  Percorso(String n, List<int> eleandata, List<int> eleritorno) {
    if (n.isEmpty || elenomi.contains(n)) {
      throw const FormatException("Names must be unique and not null");
    }
    nome = n;
    elefermateandata = eleandata;
    elefermateritorno = eleritorno;
    elenomi.add(n);
  }
}
