using System.IO;

string? path = Path.GetPathRoot(Environment.SystemDirectory);

//path = (directory.FullName).Substring(0, directory.FullName.LastIndexOf("\\"));

while (true)
{
    Console.WriteLine("Введите команду (чтобы увидеть список доступных команд, введите \"getinfo\"):");
    try
    {
        string? input = Console.ReadLine();
        string[] inputParts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (inputParts[0].ToLower() == "getinfo")
        {
            string info = """
                Список команд:
                        getinfo - список доступных команд

                        getpath - отобразить путь к текущей папке

                        goto - переход к корневой папке системного диска
                        goto путь - переход к указанной папке

                        show - отобразить все доступные элементы в текущей папке
                        show путь - перейти к папке и отобразить все доступные элементы
                        showdirs  - отобразить все подпапки в текущей папке
                        showdirs путь  - перейти к папке и отобразить все подпапки
                        showfiles - отобразить все файлы в текущей папке
                        showfiles путь - перейти к папке и отобразить все файлы

                        createdir имя - создать папку в текущей папке
                        createdir имя путь - создать папку по указанному пути
                        createfile имя - создать файл в текущей папке
                        createfile имя путь - создать файл по указанному пути

                        deletedir имя - удалить папку в текущей папке
                        deletedir имя путь - удалить папку по указанному пути
                        deletefile имя - удалить файл в текущей папке
                        deletefile имя путь - удалить файл по указанному пути

                        movedir старый_путь_или_имя новый_путь - перемещение папки
                        movefile старый_путь_или_имя новый_путь - перемещение файла

                        copyfile старый_путь_или_имя новый_путь - копирование файла

                        renamedir путь_или_имя новое_имя - переименование папки
                        renamefile путь_или_имя новое_имя - переименование файла

                        searchdirs шаблон - поиск папок в текущей папке
                        searchdirs шаблон путь - поиск папки по указанному пути
                        searchfiles шаблон - поиск файла в текущей папке
                        searchfiles шаблон путь - поиск файла по указанному пути
                            Шаблон:
                                ? - заменяет один символ
                                * - заменяет любое количество символов
                """;
            Console.WriteLine(info);
        }
        else if (inputParts[0].ToLower() == "goto")
        {
            string tempPath = "";
            if (inputParts.Length == 2)
            {
                if (Path.IsPathRooted(inputParts[1]))
                {
                    tempPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    tempPath = Path.Combine(path, inputParts[1]);
                }
            }
            else
            {
                tempPath = Path.GetPathRoot(Environment.SystemDirectory);
            }
            DirectoryInfo? directory = new DirectoryInfo(tempPath);
            if (directory.Exists)
            {
                path = directory.FullName;
            }
            else
            {
                ShowError("Папки не существует. Проверьте, правильно ли указан путь.");
            }
        }
        else if (inputParts[0].ToLower() == "show")
        {
            if (inputParts.Length == 2)
            {
                path = Path.GetFullPath(inputParts[1]);
            }
            DirectoryInfo? directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                string[] entries = Directory.GetFileSystemEntries(path);
                if (entries.Length > 0)
                {
                    foreach (string entry in entries)
                    {
                        Console.WriteLine(entry.Substring(entry.LastIndexOf('\\') + 1));
                    }
                }
                else
                {
                    Console.WriteLine("Папка пуста");
                }
                path = directory.FullName;
            }
            else
            {
                ShowError("Папки не существует. Проверьте, правильно ли указан путь.");
            }
        }
        else if (inputParts[0].ToLower() == "showdirs")
        {
            if (inputParts.Length == 2)
            {
                path = Path.GetFullPath(inputParts[1]);
            }
            DirectoryInfo? directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                DirectoryInfo[] entries = directory.GetDirectories();
                if (entries.Length > 0)
                {
                    foreach (DirectoryInfo entry in entries)
                    {
                        string dir = entry.Name;
                        Console.WriteLine(dir);
                    }
                }
                else
                {
                    Console.WriteLine("Папка пуста");
                }
                path = directory.FullName;
            }
            else
            {
                ShowError("Папки не существует. Проверьте, правильно ли указан путь.");
            }
        }
        else if (inputParts[0].ToLower() == "showfiles")
        {
            if (inputParts.Length == 2)
            {
                path = Path.GetFullPath(inputParts[1]);
            }
            DirectoryInfo? directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                FileInfo[] entries = directory.GetFiles();
                if (entries.Length > 0)
                {
                    foreach (FileInfo entry in entries)
                    {
                        string file = entry.Name;
                        Console.WriteLine(file);
                    }
                }
                else
                {
                    Console.WriteLine("Папка пуста");
                }
                path = directory.FullName;
            }
            else
            {
                ShowError("Папки не существует. Проверьте, правильно ли указан путь.");
            }
        }
        else if (inputParts[0].ToLower() == "createdir")
        {
            string dirName = "";
            if (inputParts.Length == 2)
            {
                dirName = inputParts[1];
            }
            else if (inputParts.Length == 3)
            {
                path = Path.GetFullPath(inputParts[2]);
                dirName = inputParts[1];
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            string dirPath = Path.Combine(path, dirName);
            DirectoryInfo directory = new DirectoryInfo(dirPath);
            if (!directory.Exists)
            {
                directory.Create();
                Console.WriteLine($"Папка {dirName} создана.");
            }
            else if (directory.Exists)
            {
                ShowError($"Папка {dirName} уже существует!");
            }
        }
        else if (inputParts[0].ToLower() == "createfile")
        {
            string fileName = "";
            if (inputParts.Length == 2)
            {
                fileName = inputParts[1];
            }
            else if (inputParts.Length == 3)
            {
                path = Path.GetFullPath(inputParts[2]);
                fileName = inputParts[1];
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            string filePath = Path.Combine(path, fileName);
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                file.Create();
                Console.WriteLine($"Файл {fileName} создан.");
            }
            else if (file.Exists)
            {
                ShowError($"Файл {file.Name} уже существует!");
            }
        }
        else if (inputParts[0].ToLower() == "deletedir")
        {
            string dirName = "";
            if (inputParts.Length == 2)
            {
                dirName = inputParts[1];
            }
            else if (inputParts.Length == 3)
            {
                path = Path.GetFullPath(inputParts[2]);
                dirName = inputParts[1];
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            string dirPath = Path.Combine(path, dirName);
            DirectoryInfo directory = new DirectoryInfo(dirPath);
            if (directory.Exists)
            {
                ShowError($"Это действие удалит папку {dirName} и все содержащиеся в ней файлы. Подтвердить? Да/Нет (Y/N)");
                string confirm = Console.ReadLine().ToLower();
                if (confirm == "y" || confirm == "да")
                {
                    directory.Delete(true);
                    Console.WriteLine($"Папка {dirName} удалена");
                }
                else if (confirm == "n" || confirm == "нет")
                {
                    Console.WriteLine("Удаление отменено.");
                }
                else
                {
                    ShowError("Команда не распознана. Удаление отменено.");
                }
            }
            else if (!directory.Exists)
            {
                ShowError($"Папка {dirName} не существует!");
            }
        }
        else if (inputParts[0].ToLower() == "deletefile")
        {
            string fileName = "";
            if (inputParts.Length == 2)
            {
                fileName = inputParts[1];
            }
            else if (inputParts.Length == 3)
            {
                path = Path.GetFullPath(inputParts[2]);
                fileName = inputParts[1];
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            string filePath = Path.Combine(path, fileName);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                ShowError($"Это действие удалит файл {fileName}. Подтвердить? Да/Нет (Y/N)");
                string confirm = Console.ReadLine().ToLower();
                if (confirm == "y" || confirm == "да")
                {
                    try
                    {
                        file.Delete();
                        Console.WriteLine($"Файл {fileName} удален");
                    }
                    catch (Exception error)
                    {
                        ShowError("Ошибка удаления. Удаление отменено.");
                        ShowError(error.Message);
                    }

                }
                else if (confirm == "n" || confirm == "нет")
                {
                    Console.WriteLine("Удаление отменено.");
                }
                else
                {
                    ShowError("Команда не распознана. Удаление отменено.");
                }
            }
            else if (!file.Exists)
            {
                ShowError($"Файл {fileName} не существует!");
            }
        }
        else if (inputParts[0].ToLower() == "movedir")
        {
            string oldPath = "";
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.Combine(path, inputParts[1]);
                }
                DirectoryInfo directory = new DirectoryInfo(oldPath);
                string newPath = Path.Combine(Path.GetFullPath(inputParts[2]), directory.Name);
                if (directory.Exists && !Directory.Exists(newPath))
                {
                    directory.MoveTo(newPath);
                    Console.WriteLine("Перемещение выполнено.");
                }
                else
                {
                    ShowError($"Перемещение не выполнено. Папка {directory.Name} не существует или в конечном расположении уже есть папка с именем {directory.Name}.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        else if (inputParts[0].ToLower() == "movefile")
        {
            string oldPath = "";
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.Combine(path, inputParts[1]);
                }
                FileInfo file = new FileInfo(oldPath);
                string newPath = Path.Combine(Path.GetFullPath(inputParts[2]), file.Name);
                if (file.Exists)
                {
                    bool overwrite = false;
                    if (File.Exists(newPath))
                    {
                        ShowError($"{file.Name} уже существует. Заменить? Да/Нет (Y/N)");
                        string confirm = Console.ReadLine().ToLower();
                        if (confirm == "y" || confirm == "да")
                        {
                            overwrite = true;
                        }
                        else if (confirm == "n" || confirm == "нет")
                        {
                            overwrite = false;
                            Console.WriteLine("Перемещение отменено.");
                        }
                        else
                        {
                            ShowError("Команда не распознана. Перемещение отменено.");
                        }
                    }
                    file.MoveTo(newPath, overwrite);
                    Console.WriteLine("Перемещение выполнено.");
                }
                else
                {
                    ShowError($"Перемещение не выполнено. Указанный файл не существует.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        /*
        else if (inputParts[0].ToLower() == "copydir")
        {
            string oldPath = "";
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.Combine(path, inputParts[1]);
                }
                DirectoryInfo directory = new DirectoryInfo(oldPath);
                string newPath = Path.Combine(Path.GetFullPath(inputParts[2]), directory.Name);
                if (directory.Exists && !Directory.Exists(newPath))
                {
                    directory.MoveTo(newPath);
                    Console.WriteLine("Перемещение выполнено.");
                }
                else
                {
                    ShowError($"Перемещение не выполнено. Папка {directory.Name} не существует или в конечном расположении уже есть папка с именем {directory.Name}.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        */
        else if (inputParts[0].ToLower() == "copyfile")
        {
            string oldPath = "";
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    oldPath = Path.Combine(path, inputParts[1]);
                }
                FileInfo file = new FileInfo(oldPath);
                string newPath = Path.Combine(Path.GetFullPath(inputParts[2]), file.Name);
                if (file.Exists)
                {
                    bool overwrite = false;
                    if (File.Exists(newPath))
                    {
                        ShowError($"{file.Name} уже существует. Заменить? Да/Нет (Y/N)");
                        string confirm = Console.ReadLine().ToLower();
                        if (confirm == "y" || confirm == "да")
                        {
                            overwrite = true;
                        }
                        else if (confirm == "n" || confirm == "нет")
                        {
                            overwrite = false;
                            Console.WriteLine("Копирование отменено.");
                        }
                        else
                        {
                            ShowError("Команда не распознана. Копирование отменено.");
                        }
                    }
                    file.CopyTo(newPath, overwrite);
                    Console.WriteLine("Копирование выполнено.");
                }
                else
                {
                    ShowError($"Копирование не выполнено. Указанный файл не существует.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        else if (inputParts[0].ToLower() == "renamedir")
        {
            string dirPath = "";
            if (inputParts.Length == 3)
            {
                string newName = inputParts[2];
                if (Path.IsPathRooted(inputParts[1]))
                {
                    dirPath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    dirPath = Path.Combine(path, inputParts[1]);
                }
                DirectoryInfo directory = new DirectoryInfo(dirPath);
                string oldName = directory.Name;
                string newPath = Path.Combine(directory.Parent.ToString(), newName);
                if (directory.Exists && !Directory.Exists(newPath))
                {
                    directory.MoveTo(newPath);
                    Console.WriteLine($"Папка {oldName} переименована в {directory.Name}.");
                }
                else
                {
                    ShowError($"Переименование не выполнено. Папка {newName} уже существует или не существует папки с именем {oldName}.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        else if (inputParts[0].ToLower() == "renamefile")
        {
            string filePath = "";
            if (inputParts.Length == 3)
            {
                string newName = inputParts[2];
                if (Path.IsPathRooted(inputParts[1]))
                {
                    filePath = Path.GetFullPath(inputParts[1]);
                }
                else if (!Path.IsPathRooted(inputParts[1]))
                {
                    filePath = Path.Combine(path, inputParts[1]);
                }
                FileInfo file = new FileInfo(filePath);
                string oldName = file.Name;
                string newPath = Path.Combine(file.DirectoryName, newName);
                if (file.Exists && !File.Exists(newPath))
                {
                    file.MoveTo(newPath, false);
                    Console.WriteLine($"Файл {oldName} переименован в {file.Name}.");
                }
                else
                {
                    ShowError($"Переименование не выполнено. Файл {newName} уже существует или не существует файла с именем {oldName}.");
                }
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
        }
        else if (inputParts[0].ToLower() == "searchdirs")
        {
            string dirPath = "";
            string template = inputParts[1];
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[2]))
                {
                    dirPath = Path.GetFullPath(inputParts[2]);
                }
                else if (!Path.IsPathRooted(inputParts[2]))
                {
                    dirPath = Path.Combine(path, inputParts[2]);
                }
            }
            else if (inputParts.Length == 2)
            {
                dirPath = path;
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            SearchDirs(dirPath, template);
        }
        else if (inputParts[0].ToLower() == "searchfiles")
        {
            string filePath = "";
            string template = inputParts[1];
            if (inputParts.Length == 3)
            {
                if (Path.IsPathRooted(inputParts[2]))
                {
                    filePath = Path.GetFullPath(inputParts[2]);
                }
                else if (!Path.IsPathRooted(inputParts[2]))
                {
                    filePath = Path.Combine(path, inputParts[2]);
                }
            }
            else if (inputParts.Length == 2)
            {
                filePath = path;
            }
            else
            {
                ShowError("Проверьте правильность ввода команды!");
            }
            SearchFiles(filePath, template);
        }
        else if (inputParts[0].ToLower() == "getpath")
        {
            Console.WriteLine(path);
        }
        else if (inputParts[0].ToLower() == "exit")
        {
            break;
        }
        else
        {
            ShowError("Неверная команда!");
        }
    }
    catch (Exception e)
    {
        ShowError(e.Message);
    }
}

void ShowError(string message)
{
    //Console.BackgroundColor = ConsoleColor.Red;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
}

void SearchDirs(string dirPath, string template)
{
    if (Directory.Exists(dirPath))
    {
        string[] allDirectories = Directory.GetDirectories(dirPath);
        if (allDirectories.Length > 0)
        {
            string[] directories = Directory.GetDirectories(dirPath, template);
            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
            }
            foreach (string directory in allDirectories)
            {
                SearchDirs(directory, template);
            }
        }
    }

}

void SearchFiles(string filePath, string template)
{
    if (Directory.Exists(filePath))
    {
        string[] directories = Directory.GetDirectories(filePath);
        if (directories.Length > 0)
        {
            string[] files = Directory.GetFiles(filePath, template);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
            foreach (string directory in directories)
            {
                SearchFiles(directory, template);
            }
        }
    }

}