import 'package:http/http.dart' as http;
import 'dart:convert';


class Webhook {

  Future<void> SendDrink(String name, String alcCl) async {
    int cupVolume = 10;
    String soda;
    if (name.contains("Shot")){
      soda = "0";
    }else {
      int.parse(alcCl) - cupVolume;
      soda = "$cupVolume";
    }
    final response = await http.post(
      Uri.parse('https://10.0.2.2:7150/api/Tester/test'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(<String, dynamic>{
        'name': name,
        'alcCl': double.parse(alcCl),
        'sodaCl': double.parse(soda),
      }),
    );
    print(alcCl);
    print(response.statusCode);
  } 
}