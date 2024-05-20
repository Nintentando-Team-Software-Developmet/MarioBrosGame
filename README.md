
# Super Mario Bros. Remake (Software Development V)

This project is a remake of the classic Super Mario Bros. game for educational purposes in Software Development V.

## Team Members:
- Gaston Gutierrez
- Karina Aguirre
- Isaias Rojas
- Juan Pablo Arequipa
- Hugo Oropeza
- Fernando Mauricio Mamani Navarro



Here's a README that explains how to run tests and the project in different folders, and also how to run the project in the root directory using a `--project` flag:

---

## Running Tests

To run tests in a specific folder, navigate to that folder and execute the following command (you can run the Test on the root folder, the MarioGame and MarioGame.Test):

```bash
dotnet test
```

This command will run all the tests in the folder.

## Running the Project

To run the project in a specific folder, navigate to that folder and execute the following command:

```bash
dotnet run
```

The `Program.cs` file in the folder you're in will be executed.

### Note:
- Ensure that the `Program.cs` file in the folder you're running reflects the desired functionality of the project.
- You can run tests and the project in each folder independently (MarioGame and MarioGame.Test), and each `Program.cs` in the `test` folder should reflect the `Program.cs` in the `run` folder.

## Running the Project in the Root Directory

To run the project in the root directory, you need to use the `--project` flag followed by the path to the `Program.cs` file. Execute the following command:

```bash
dotnet run --project MarioGame
```

Replace `<path_to_program_cs>` with the path to the `Program.cs` file you want to run.


---

Feel free to customize this README further based on your project's structure and requirements.


## License:

This project is licensed under the MIT LICENSE. You can find the full license details in the LICENSE file. (Common open-source licenses for educational projects include MIT or Apache 2.0. Choose the one that best suits your team's needs.)


This project is a re-imagining of Super Mario Bros. for educational purposes only. It is not affiliated with Nintendo in any way.

