// ignore_for_file: prefer_const_constructors, prefer_const_literals_to_create_immutables, non_constant_identifier_names

import 'dart:io';
import 'package:flutter/material.dart';
import 'webhook.dart';

class MyHttpOverrides extends HttpOverrides{
  @override
  HttpClient createHttpClient(SecurityContext? context){
    return super.createHttpClient(context)
      ..badCertificateCallback = (X509Certificate cert, String host, int port)=> true;
  }
}

void main(){
  HttpOverrides.global = MyHttpOverrides();
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const MyHomePage(title: 'Flutter Demo Home Page'),
    );

  }
}

class CustomButton extends ElevatedButton {
  CustomButton({
    required VoidCallback onPressed,
    required String text,
  }) : super(
          onPressed: onPressed,
          child: Text(text, style: TextStyle(fontSize: 20),),
          style: ButtonStyle(
            minimumSize: MaterialStateProperty.all<Size>(
              Size(150, 60), // set the desired width and height here
            ),
          ),
        );
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});
  final String title;
  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  DrinkLabel? selectedDrink;

  TextEditingController textController = TextEditingController();
  TextEditingController drinkTypeController = TextEditingController();
  String drinkType = "";

  Webhook webhook = Webhook();

  @override
  void dispose() {
    // Clean up the controller when the widget is disposed.
    drinkTypeController.dispose();
    textController.dispose();
    super.dispose();
  }

  void SendDrink(){
    if (num.parse(textController.text) >= 0.5 && num.parse(textController.text) <= 4.0 && textController.text != null){
      webhook.SendDrink(selectedDrink!.label, textController.text);
      
    }
    
  }

  @override
  Widget build(BuildContext context) {
    final List<DropdownMenuEntry<DrinkLabel>> colorEntries = <DropdownMenuEntry<DrinkLabel>>[];
    for (final DrinkLabel color in DrinkLabel.values) {
      colorEntries.add(DropdownMenuEntry<DrinkLabel>(value: color, label: color.label, enabled: color.label != 'Grey'));
    }
    return Scaffold(
      body: Column(
        children: [
          Container(
            color: Colors.black,
            height: 49.4,
            width: (MediaQuery.of(context).size.width),
          ),
          Container(
            height: 10,
            width: (MediaQuery.of(context).size.width),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Text("Drinks: ",
                style: TextStyle(
                  fontSize: 30,
                ),
              ),
              DropdownMenu<DrinkLabel>(
                    initialSelection: DrinkLabel.romCola,
                    controller: drinkTypeController,
                    label: const Text('Drinks'),
                    dropdownMenuEntries: colorEntries,
                    onSelected: (DrinkLabel? label) {
                      setState(() {
                        selectedDrink = label;
                      });
                    },
                  ),
            ],
          ),
          Container(
            height: 20,
            width: (MediaQuery.of(context).size.width),
          ),
          Container(
            height: 60,
            width: (MediaQuery.of(context).size.width - 80),
            child: TextField(
                      controller: textController,
                      decoration: InputDecoration(
                        border: OutlineInputBorder(),
                        labelText: 'Centiliter Alcohol -- Min 0.5cl | Max 4cl'
                      ),
                    ),
          ),
          Container(
            height: 20,
            width: (MediaQuery.of(context).size.width),
          ),
          CustomButton(onPressed: SendDrink, text: "Mix Drink!")
        ],
      ),
    );
  }
}


enum DrinkLabel {
  romCola('Rom & Cola'),
  romShot('Rom Shot');

  const DrinkLabel(this.label);
  final String label;
}