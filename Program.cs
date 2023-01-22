using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace IdS2_LabDependencias_1172819
{
    class Program
    {
        static void Main(string[] args)
        {
            //Nombres generados al azar
            string[] apellidos = { "Mejia", "Hernandez", "González", "Rodríguez", "Fernández", "Díaz", "López", "García", "Pérez", "Velázquez", "Cruz", "Samson","Samuel"
            ,"Samuele","Sancho","Sander","Sanders","Sanderson",
            "Sandor","Sandro","Sandy","Sanford","Sanson","Sansone","Sarge","Sargent","Sascha","Sasha","Saul","Sauncho","Saunder","Saunders","Saunderson","Saundra","Sauveur","Saw","Sawyer","Sawyere","Sax","Saxe","Saxon","Say","Sayer","Sayers","Sayre","Sayres","Scarface","Schuyler","Scot","Scott","Scotti","Scottie","Scotty","Seamus","Sean"
            ,"Sebastian","Sebastiano","Sebastien","See","Selby","Selig","Serge","Sergeant","Sergei","Sergent","Sergio","Seth","Seumas","Seward","Seymour","Shadow","Shae","Shaine","Shalom","Shamus","Shanan","Shane","Shannan","Shannon","Shaughn","Shaun","Shaw","Shawn","Shay","Shayne","Shea","Sheff","Sheffie","Sheffield","Sheffy" };

            var serviceProvider = new ServiceCollection()
            .AddTransient<ITransientService, Asientos>()
            .AddScoped<IScopedService, Sala>()
            .AddSingleton<ISingletonService, Cine>()
            .BuildServiceProvider();

            bool repeat = false;

            Console.WriteLine("Ingrese el nombre del cine que manejara:");
            serviceProvider.GetService<ISingletonService>().setNombre(Console.ReadLine());
            
            do {
                repeat = false;
                using (IServiceScope scope = serviceProvider.CreateScope()) {
                    Console.WriteLine("Estado Sala: ");
                    Console.WriteLine("Funcion: " + scope.ServiceProvider.GetService<IScopedService>().getFuncion());
                    Console.WriteLine("Asientos Vendidos: " + scope.ServiceProvider.GetService<IScopedService>().getAsientos().Count);
                    Console.WriteLine("Que pelicula sera vista en la sala 1?");
                    string pelicula = Console.ReadLine();
                    scope.ServiceProvider.GetService<IScopedService>().setFuncion(pelicula);
                    List<ITransientService> lista = new List<ITransientService>();
                    for (int i = 0; i < 45; i++)
                    {
                        Random r = new Random();
                        lista.Add(scope.ServiceProvider.GetService<ITransientService>());
                        lista[i].setAsiento(r.Next(40000, 50000).ToString(), apellidos[r.Next(apellidos.Length)]);
                    }
                    scope.ServiceProvider.GetService<IScopedService>().setAsientos(lista);
                    Console.WriteLine("La funcion en el cine " + serviceProvider.GetService<ISingletonService>().getNombre() +" En la sala 1 sera:");
                    Console.WriteLine(serviceProvider.GetService<IScopedService>().getFuncion());
                    Console.WriteLine("Y los asientos vendidos son: ");
                    scope.ServiceProvider.GetService<IScopedService>().printAsientos();
                    
                }
                Console.WriteLine("Reiniciar Funcion? S/N");
                string respuesta = Console.ReadLine();

                if (respuesta == "S")
                    repeat = true;
            } while (repeat);
        }
    }

    

    public interface ITransientService
    {
        void setAsiento(string code, string client);
        string getCodigo();
        string getCliente();

    }

    public interface IScopedService
    {
        void setAsientos(List<ITransientService> lista);

        List<ITransientService> getAsientos();
        void setFuncion(string nombre);

        string getFuncion();

        void printAsientos();

    }

    public interface ISingletonService
    {
        IScopedService getSala();
        void SetSala(IScopedService sala1);

        string getNombre();
        void setNombre(string name);
    }

    public class Cine : ISingletonService {
        public Cine() {}
        IScopedService Sala;
        public string nombre;

        public void SetSala(IScopedService sala1) {
            Sala = sala1;
        }

        public void setNombre(string name) {
            nombre = name;
        }

        public IScopedService getSala()
        {
            return Sala;
        }

        public string getNombre() {
            return nombre;
        }
    }



    public class Sala : IScopedService {

        List<ITransientService> asientos = new List<ITransientService>();
        string funcion;
        public Sala() { }

        public void setFuncion(string nombre) {
            funcion = nombre;
        }
        public void setAsientos(List<ITransientService> lista) {
            asientos = lista;
        }

        public List<ITransientService> getAsientos()
        {
            return asientos;
        }

        public string getFuncion()
        {
            return funcion;
        }

        public void printAsientos()
        {
            foreach (var item in asientos)
            {
                Console.WriteLine("Codigo: " + item.getCodigo() + ", Cliente: " + item.getCliente());
            }
        }

    }

    public class Asientos : ITransientService {
        public string codigo;
        public string cliente;
        public Asientos() { }

        public void setAsiento(string code, string client)
        {
            codigo = code;
            cliente = client;
        }

        public string getCodigo() {
            return codigo;
        }

        public string getCliente()
        {
            return cliente;
        }

    }
    }
