using System;
using System.Collections.Generic;
using System.IO;

namespace fruta
{
    interface IFruta
    {
        string Nombre { get; }
        string Color { get; }
    }

    class Manzana : IFruta
    {
        public string Nombre => "Manzana";
        public string Color => "Rojo";
    }

    class Pera : IFruta
    {
        public string Nombre => "Pera";
        public string Color => "Verde";
    }

    class Banana : IFruta
    {
        public string Nombre => "Banana";
        public string Color => "Amarillo";
    }

    class Naranja : IFruta
    {
        public string Nombre => "Naranja";
        public string Color => "Naranja";
    }

    class Program
    {
        private static string archivoFrutas = "frutas.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("---------FRUTAS--------");

            List<IFruta> frutas = new List<IFruta>
            {
                new Manzana(),
                new Pera(),
                new Banana(),
                new Naranja()
            };

            GuardarEnArchivo(frutas, archivoFrutas);

            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("\nMenú:");
                Console.WriteLine("1. Mostrar lista de frutas");
                Console.WriteLine("2. Agregar una fruta");
                Console.WriteLine("3. Eliminar una fruta");
                Console.WriteLine("4. Crear archivo de texto");
                Console.WriteLine("5. Guardar lista de frutas en archivo de texto");
                Console.WriteLine("6. Modificar archivo existente");
                Console.WriteLine("7. Salir");

                Console.Write("\nSelecciona una opción: ");
                string opcion = Console.ReadLine();

                Console.Clear();

                switch (opcion)
                {
                    case "1":
                        MostrarFrutas(frutas);
                        break;
                    case "2":
                        AgregarFruta(frutas);
                        break;
                    case "3":
                        EliminarFruta(frutas);
                        break;
                    case "4":
                        CrearArchivo(frutas);
                        break;
                    case "5":
                        GuardarEnArchivo(frutas, archivoFrutas);
                        break;
                    case "6":
                        ModificarArchivo(frutas);
                        break;
                    case "7":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                        break;
                }
            }
        }

        static void MostrarFrutas(List<IFruta> frutas)
        {
            Console.WriteLine("\nLista de Frutas:");
            foreach (var fruta in frutas)
            {
                Console.WriteLine($"Nombre: {fruta.Nombre}, Color: {fruta.Color}");
            }
        }

        static void AgregarFruta(List<IFruta> frutas)
        {
            Console.Write("\nIngrese el nombre de la fruta: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el color de la fruta: ");
            string color = Console.ReadLine();

            IFruta nuevaFruta = new FrutaPersonalizada(nombre, color);
            frutas.Add(nuevaFruta);

            GuardarEnArchivo(frutas, archivoFrutas);

            Console.WriteLine("Fruta agregada con éxito.");
        }

        static void EliminarFruta(List<IFruta> frutas)
        {
            Console.Write("\nIngrese el nombre de la fruta que desea eliminar: ");
            string nombre = Console.ReadLine();

            IFruta frutaEncontrada = frutas.Find(fruta => fruta.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (frutaEncontrada != null)
            {
                frutas.Remove(frutaEncontrada);
                GuardarEnArchivo(frutas, archivoFrutas);
                Console.WriteLine("Fruta eliminada con éxito.");
            }
            else
            {
                Console.WriteLine("Fruta no encontrada.");
            }
        }


        static void CrearArchivo(List<IFruta> frutas)
        {
            int contador = 1;
            string nombreArchivoBase = "frutas";
            string nombreArchivo = $"{nombreArchivoBase}_{contador:00}.txt";

            while (File.Exists(nombreArchivo))
            {
                contador++;
                nombreArchivo = $"{nombreArchivoBase}_{contador:00}.txt";
            }

            GuardarEnArchivo(frutas, nombreArchivo);
            Console.WriteLine($"Archivo '{nombreArchivo}' creado con éxito en la ruta: {Path.GetFullPath(nombreArchivo)}");
        }


        static void ModificarArchivo(List<IFruta> frutas)
        {
            Console.Write("Ingrese el nombre del archivo a modificar (con la extensión .txt): ");
            string nombreArchivo = Console.ReadLine();

            if (File.Exists(nombreArchivo))
            {
                CargarDesdeArchivo(frutas, nombreArchivo);
                Console.WriteLine($"Archivo '{nombreArchivo}' cargado con éxito.");
            }
            else
            {
                Console.WriteLine("El archivo no existe.");
            }
        }

        static void CargarDesdeArchivo(List<IFruta> frutas, string nombreArchivo)
        {
            frutas.Clear();

            using (StreamReader sr = new StreamReader(nombreArchivo))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] partes = linea.Split(',');
                    if (partes.Length == 2)
                    {
                        string nombre = partes[0].Trim().Substring(8); // Eliminar "Nombre: "
                        string color = partes[1].Trim().Substring(7); // Eliminar "Color: "
                        IFruta fruta = new FrutaPersonalizada(nombre, color);
                        frutas.Add(fruta);
                    }
                }
            }
        }

        static void GuardarEnArchivo(List<IFruta> frutas, string nombreArchivo)
        {
            using (StreamWriter sw = new StreamWriter(nombreArchivo))
            {
                foreach (var fruta in frutas)
                {
                    sw.WriteLine($"Nombre: {fruta.Nombre}, Color: {fruta.Color}");
                }
            }

            Console.WriteLine($"Lista de frutas guardada con éxito en el archivo: {Path.GetFullPath(nombreArchivo)}");
        }

        class FrutaPersonalizada : IFruta
        {
            public string Nombre { get; }
            public string Color { get; }

            public FrutaPersonalizada(string nombre, string color)
            {
                Nombre = nombre;
                Color = color;
            }
        }
    }
}