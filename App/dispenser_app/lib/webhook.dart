import 'package:http/http.dart' as http;
import 'dart:convert';
import 'dart:math';

class Webhook {

  Future<void> SendDrink(String name, num alcCl) async {
    num cupVolume = 30;
    num soda;
    if (name.contains("Shot")){
      soda = 0;
    }else {
      cupVolume = cupVolume - alcCl;
      soda = cupVolume;
    }
    final response = await http.post(
      Uri.parse('https://10.0.2.2:7150/api/Tester/test'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(<String, dynamic>{
        'name': name,
        'alcCl': double.parse(alcCl.toString()),
        'sodaCl': double.parse(soda.toString()),
      }),
    );
    print(alcCl);
    print(soda);
    print(response.statusCode);
  } 
}
