using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        static Dictionary<Type, object> recievedOperation = new Dictionary<Type, object>();
        static Dictionary<Type, object> recievedData = new Dictionary<Type, object>();
        static BinaryMessage operationToSend = new BinaryMessage();
        static BinaryMessage dataToSend = new BinaryMessage();
        static List<Teacher> teachers = new List<Teacher>();
        static List<Subject> subjects = new List<Subject>();

        static void Main(string[] args)
        {
            try
            {
                ServerManager.InitializeServer();

                while (true)
                {
                    ServerManager.ListenForNewConnections();

                    do
                    {
                        recievedOperation = ServerManager.WaitForMessage(); 
                        recievedData = ServerManager.WaitForMessage(); 

                        ProcessClientOperation();

                        if (!ServerManager.CommunicationIsActive)
                        {
                           
                            break;
                        }
                    } while (true);

                    if (!ServerManager.ContinueListening())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to close the program");
                Console.ReadKey();
            }
            finally
            {
                ServerManager.CloseConnection();
            }
        }

        private static void ProcessClientOperation()
        {
            
            Subject subject = null;
            Teacher teacher = null;
            int? index;

            int? operation = recievedOperation[typeof(Int32)] as int?;

            switch (operation)
            {
                case 1:
                    teacher = recievedData[typeof(Teacher)] as Teacher;
                    teachers.Add(teacher);
                    Console.WriteLine("User added successfully!");
                    break;

                case 2:
                    subject = recievedData[typeof(Subject)] as Subject;
                    subjects.Add(subject);
                    Console.WriteLine("Message added successfully!");
                    break;

                case 3:
                    dataToSend = TransformDataManager.Serialize(teachers);
                    ServerManager.SendMessage(dataToSend);
                    break;

                case 4:
                    dataToSend = TransformDataManager.Serialize(subjects);
                    ServerManager.SendMessage(dataToSend);
                    break;

                
                case 5:
                    ServerManager.CommunicationIsActive = false;
                    break;

                default:
                    break;
            }

        }

        public static void EndCommunication()
        {
            operationToSend = TransformDataManager.Serialize(5);
            dataToSend = TransformDataManager.Serialize(string.Empty);

            ServerManager.SendMessage(operationToSend);
            ServerManager.SendMessage(dataToSend);
        }

    }
}
