using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using res2source.viewmodels;
using System.Text.RegularExpressions;

namespace res2source
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        static MainWindowViewModel model = new MainWindowViewModel
        {
            ClassName = "UnnamedClass"
        };
        public MainWindow()
        {
            InitializeComponent();
            // if (model.Files.Count == 0)
            // {
            // var files = Directory.GetFiles(@"C:\Users\James\Downloads\Programs").Where(e => File.Exists(e)).Select(e => new FileObject { ObjectName = Path.GetFileNameWithoutExtension(e), Path = e });
            // model.Files = new System.Collections.ObjectModel.ObservableCollection<FileObject>(files);
            DataContext = model;
            // }

        }
        public static string FirstCharToUpperAsSpan(string input)
        {
            input = input.Trim();
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            if (input.Length > 0)
            {
                if (Char.IsAsciiLetter(input[0]) && Char.IsLower(input[0]))
                {
                    var arr = input.ToCharArray();
                    arr[0] = Char.ToUpper(input[0]);
                    input = new string(arr);
                }
            }

            return input;
        }
        public static string SanitizeName(string name)
        {
            var names = Regex.Split(name.Trim(), "[\\s]+").Select(el => el.Trim()).Where(e => e.Length > 0).ToArray();
            if (names.Length > 1)
            {
                names = names.Select(e => FirstCharToUpperAsSpan(Regex.Replace(e.Trim(), "[^A-Za-z\\d]+", "_"))).ToArray();

                name = string.Join("", names);
            }
            else
            {
                name = FirstCharToUpperAsSpan(Regex.Replace(name.Trim(), "[^A-Za-z\\d]+", "_"));

            }
            return name;
        }
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Control)sender).Tag;
            if (tag is FileObject)
            {
                var obj = tag as FileObject;
                if (obj != null)
                {
                    model.Files.Remove(obj);
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = model.ClassName + ".h";
            dialog.Filter = "C++ header files |*.h";

            if (dialog.ShowDialog() ?? false)
            {
                var headerFile = dialog.FileName;
                if (model.Files.Count > 0)
                {
                    var classTemplate = """
#pragma once
#include <vector>
using std::vector;
namespace resource
{
    class ClassName
    {
    public:
        |template|
    };
}
""";
                    classTemplate = Regex.Replace(classTemplate, "ClassName", model.ClassName);
                    var classSplit = Regex.Split(classTemplate, "\\|template\\|", RegexOptions.Multiline);

                    using (var export = File.Open(headerFile, FileMode.Create))
                    {
                        export.Write(Encoding.UTF8.GetBytes(classSplit[0]));
                        foreach (var file in model.Files)
                        {
                            export.Write(Encoding.UTF8.GetBytes("\r\n"));
                            var methodTemplate = """
        static std::vector<unsigned char> method()
        {
            std::vector<unsigned char> result;
            const char buffer[|size|] = {
                 |bytesTemplate| 
            };
            result.resize(sizeof(buffer));
            memcpy(&result[0], buffer, sizeof(buffer));
            return result;
        }
""";
                            methodTemplate = Regex.Replace(methodTemplate, "method", "Get" + file.ObjectName);
                            methodTemplate = Regex.Replace(methodTemplate, "\\|size\\|", $"{file.Size}");
                            var methodSplit = Regex.Split(methodTemplate, "\\|bytesTemplate\\|", RegexOptions.Multiline);

                            export.Write(Encoding.UTF8.GetBytes(methodSplit[0].TrimEnd()));


                            using (var input = File.OpenRead(file.Path))
                            {
                                export.Write(Encoding.UTF8.GetBytes("\r\n"));
                                var buffer = new byte[1024];
                                var dummybuffer = new byte[1];
                                var read = 0;
                                export.Write(Encoding.UTF8.GetBytes("\t\t\t\t"));
                                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (read > 0)
                                    {
                                        for (int i = 0; i < read; i++)
                                        {
                                            dummybuffer[0] = buffer[i];
                                            export.Write(Encoding.UTF8.GetBytes("OX" + Convert.ToHexString(dummybuffer)));
                                            if (i < (read - 1))
                                            {
                                                export.WriteByte((byte)',');
                                            }
                                            if (((i + 1) % 16) == 0)
                                            {
                                                export.WriteByte((byte)'\n');
                                                export.Write(Encoding.UTF8.GetBytes("\t\t\t\t"));
                                            }

                                        }
                                    }
                                }

                            }

                            export.Write(Encoding.UTF8.GetBytes(methodSplit[1].TrimStart()));
                        }
                        export.Write(Encoding.UTF8.GetBytes(classSplit[1]));
                    }
                }
            }
        }
        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Select file to  add";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() ?? false)
            {
                var files = dialog.FileNames;
                foreach (var item in files)
                {
                    if (!model.Files.Any(e => e.Path == item))
                    {
                        model.Files.Insert(0, new FileObject
                        {
                            ObjectName = SanitizeName(Path.GetFileNameWithoutExtension(item)),
                            Path = item,
                            Size = new FileInfo(item).Length
                        });
                    }
                }
            }

        }
    }
}
