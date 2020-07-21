using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;


using System.Reflection; // для дебага
using System.IO;



using MySql.Data.MySqlClient; // MySQL по ссылке


// 180121 = 1 сборка
//# using static UNIV_CLASS_namespace.UNIV_CLASS; // Для подключения DLL, вызов функ по ИМЕНИ
//# Добавить ССЫЛКУ на DLL из Debug в нужном проекте


namespace UNIV_CLASS_namespace
{


	public static class UNIV_CLASS
	{

		static string UNIV_ENG_alphabet_big = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		static string UNIV_ENG_alphabet_sml = "abcdefghijklmnopqrstuvwxyz";

		static string UNIV_RUS_alphabet_big = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
		static string UNIV_RUS_alphabet_sml = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

		static string UNIV_ALL_nums = "0123456789";

		static string UNIV_exe_derectory = Environment.CurrentDirectory;//папка в которой лежит exe






		//########################################################################
		//########################################################################


		public static string UNIV_Browse_File( string Filter = "Все файлы|*.*")
		{
			// Сделать чтоб параметр был просто строкой с расширениями, а тут лежал массив с их описаниями

			OpenFileDialog OPF = new OpenFileDialog();

			if (Filter != "Все файлы|*.*")
				OPF.Filter = Filter + "|Все файлы|*.*"; // маска поиска
			else
				OPF.Filter = Filter;


			if (OPF.ShowDialog() == DialogResult.OK)
				return OPF.FileName;

			return null;// "Файл не выбран.";
		}
		// Вернет выбранный ПУТЬ или "НЕ ВЫБРАНО"
		// Закончено, можно улучшить(внутри)

		public static string UNIV_Browse_Folder()
		{
			FolderBrowserDialog FBD = new FolderBrowserDialog();

			if (FBD.ShowDialog() == DialogResult.OK)
				return FBD.SelectedPath;

			return null;// "Путь не выбран.";
		}
		// Вернет выбранный ПУТЬ или "НЕ ВЫБРАНО"
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ




		public static void UNIV_Create_File( string path )
		{


		}




		public static List<string> UNIV_GET_MYSQL_QUERYES_FROM_PMA_FILE(string File_path)
		{

			string[] readText = File.ReadAllLines(File_path);
			


			string all_sql = "";

			foreach (string s in readText) // построчная склейка в 1 строку
			{
				//if( s != "" )
					all_sql += s + "\n";
			}


			List<string> queryes = all_sql.Split(';').ToList();

			return queryes;

		}

		// ФАЙЛЫ

		//########################################################################
		//########################################################################





		//########################################################################
		//########################################################################

		

		public static string UNIV_GET_MYSQL_CONNECTION_STRING(string host, string name, string pass)
		{
			//проверка на пустоту аргументов

			string myConnectionString = "";

			//myConnectionString += "Database = " + db_name + ";";
			myConnectionString += "Data Source=" + host + ";";
			myConnectionString += "User Id=" + name + ";";
			myConnectionString += "Password=" + pass;

			return myConnectionString;

			//string myConnectionString = "Database=db_name;Data Source=192.168.0.1;User Id=root;Password=rootpass";


			/* MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
             mysqlCSB.Server = "192.168.0.1";  // IP адоес БД
             mysqlCSB.Database = "test_db";    // Имя БД
             mysqlCSB.UserID = "root";        // Имя пользователя БД
             mysqlCSB.Password = "rootpass";   // Пароль пользователя БД
             mysqlCSB.CharacterSet = "cp1251"; // Кодировка Базы Данных  */

		}
		// Вернет строку(точно не пустую)
		// ЗАКОНЧЕНО , дописать

		public static MySqlConnection UNIV_GET_MYSQL_CONNECT_TO_SERVER(string myConnectionString)
		{

			//if (myConnectionString is null)

			MySqlConnection myConnection = new MySqlConnection(myConnectionString);

			try
			{
				myConnection.Open(); // Открываем соединение
			}
			catch (Exception ex)
			{
				UNIV_MSG_STR(ex.Message + "\n" + myConnectionString, "Ошибка подключения к серверу.");
				return null;
			}

			return myConnection;

		}
		// Вернет коннект или NULL
		// ЗАКОНЧЕНО


		


		//########################################################################

		//Исполняет запросы без вывода(NO_DATA) или выводит в лист_лист(any char)
		// Результат Запроса в формате LIST[строка][столбец] (индексы, а не имена)
		public static List<List<string>> UNIV_MYSQL_EXEC_QUERY( MySqlConnection CONNECT , string SQL_query , string Target_DB , string MODE = "anychar" )
		{
			//################################


			//MySqlConnection connection = new MySqlConnection("Database = " + DB_name + ";" + DB_connect_string);
			if (CONNECT is null)
			{
				UNIV_MSG_STR("Не создано подключение к серверу.(return null)", "UNIV_MYSQL_EXEC_QUERY");
				return null;
			}


			//################################


			try // Что если соединение отвалилось?
			{
				if (Target_DB != "DB_NOT_NEED")
					CONNECT.ChangeDatabase(Target_DB);
			}
			catch (Exception ex)
			{
				UNIV_MSG_STR("Подкл есть, но не удалось выбрать БД(return null)\n" + ex.Message, "UNIV_MYSQL_EXEC_QUERY");
				return null;
			}


			//################################


			MySqlCommand sqlCom = new MySqlCommand(SQL_query, CONNECT);
			try
			{
				//Выполняет запрос,        вернет кол-во строк в SQL-запросе ( !!НЕ!! данных)
				sqlCom.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				UNIV_MSG_STR("Не удалось исполнить этот запрос(return null):\n" + SQL_query + "\n\n" + ex.Message, "UNIV_MYSQL_EXEC_QUERY Запрос не прошел.");
				return null;
			}


			//################################


			if (MODE == "NO_DATA")
			{
				//UNIV_MSG_STR("Запрос исполнен(NO_DATA)(return null):\n" + SQL_query, "SQL_EXEC_QUERY Запрос прошел.");
				return null;
			}


			//################################



			MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);

			DataTable dt = new DataTable();
			dataAdapter.Fill(dt);//было

			DataRow[] ALL_query_rows = dt.Select();

			//################################




			if (ALL_query_rows.Length == 0)
			{
				UNIV_MSG_STR("Выборка == 0 строк (return null):\n" + SQL_query, "UNIV_MYSQL_EXEC_QUERY Итог пуст.");
				return null;
			}



			//################################

			List<List<string>> List_all_rows = new List<List<string>>();


			foreach (DataRow one_row in ALL_query_rows)
			{

				List<string> one_full_row = new List<string>();

				for (int i = 0; i < dt.Columns.Count; i++)
				{
					one_full_row.Add(one_row[i].ToString());    // Одна ПОЛНАЯ строка
				}

				List_all_rows.Add(one_full_row); // пишем в общий

			}


			//################################

			//DEBUG_MSG_LIST_LIST(List_all_rows);

			return List_all_rows;
		}



		public static void UNIV_MYSQL_EXEC_LIST_OF_QUERYES( MySqlConnection CONNECT, List<string> Queryes, string Target_DB , ProgressBar PBAR )
		{
			PBAR.Visible = true;

			PBAR.Step = 1;
			PBAR.Value = 1;

			PBAR.Minimum = 0;
			PBAR.Maximum = Queryes.Count;

			int cnt = 1;
			foreach (string query in Queryes)
			{

				UNIV_MYSQL_EXEC_QUERY(CONNECT, query, Target_DB , "NO_DATA");

				PBAR.PerformStep();


				PBAR.Refresh();
				PBAR.CreateGraphics().DrawString( 
												cnt + " из " + PBAR.Maximum , 
												new Font("Arial", (float)7.15) , 
												Brushes.Black , 
												new PointF(PBAR.Width / 2 - 20, PBAR.Height / 2 - 7)
												);

			cnt++;
			}

		}


		//########################################################################

		public static List<string> UNIV_MYSQL_GET_SERVER_BASE_NAMES(MySqlConnection CONNECT)
		{

			string SQL_query = "SHOW DATABASES";
			List<List<string>> list_list_NAMES = UNIV_MYSQL_EXEC_QUERY( CONNECT , SQL_query, "DB_NOT_NEED" );

			if (list_list_NAMES == null)
				return null;

			return UNIV_CONVERT_LIST_LIST_IN_ONE_LIST(list_list_NAMES);



			/*
			List<string> NAMES = new List<string>();

			foreach (List<string> db_name in list_list_NAMES)
				if (db_name[0] != "information_schema" && db_name[0] != "performance_schema" && db_name[0] != "mysql")
					NAMES.Add(db_name[0]);


			return NAMES; */

		}
		// Получение имен баз
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ

		public static List<string> UNIV_MYSQL_GET_BASE_TABLES_NAMES(MySqlConnection CONNECT, string Target_BASE)
		{

			string SQL_query = "SHOW TABLES";

			List<List<string>> list_list_NAMES = UNIV_MYSQL_EXEC_QUERY(CONNECT, SQL_query, Target_BASE);

			if (list_list_NAMES == null)
				return null;

			return UNIV_CONVERT_LIST_LIST_IN_ONE_LIST(list_list_NAMES);


		}
		// Получение имен таблиц
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ

		public static List<string> UNIV_MYSQL_GET_TABLE_COLUMN_NAMES(MySqlConnection CONNECT, string Target_BASE, string Target_TABLE )
		{

			string SQL_query = "SHOW COLUMNS FROM " + Target_TABLE;
			List<List<string>> list_list_NAMES = UNIV_MYSQL_EXEC_QUERY(CONNECT, SQL_query, Target_BASE);


			// впихнуть иф на строк >= 2

			if (list_list_NAMES == null)
				return null;

			return UNIV_CONVERT_LIST_LIST_IN_ONE_LIST(list_list_NAMES);

		}
		// Получение имен столбцов
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ






		//########################################################################

		public static void UNIV_MYSQL_DROP_DB(MySqlConnection CONNECT, string Target_BASE)
		{
			string SQL_query = "DROP DATABASE " + Target_BASE ;
			UNIV_MYSQL_EXEC_QUERY(CONNECT, SQL_query, "DB_NOT_NEED", "NO_DATA");
		}

		public static void UNIV_MYSQL_CREATE_DB(MySqlConnection CONNECT, string Target_BASE)
		{
			string SQL_query = "CREATE DATABASE " + Target_BASE;
			UNIV_MYSQL_EXEC_QUERY(CONNECT, SQL_query, "DB_NOT_NEED", "NO_DATA");
		}


		//########################################################################




		//INSERT INTO table_name (колонки)   VALUES(value1, value2, value3, ...);
		// Собирает ЗАПРОС    ВСЕЙ строки бд с id=парам[0]
		public static string UNIV_GET_MYSQL_QUERY_CONSTRUCTOR_INSERT_ROW(string TABLE, List<string> LIST_COLUMNS, List<string> LIST_VALUES)
		{

			//List<string> columns = UNIV_MYSQL_GET_TABLE_COLUMN_NAMES(GL_my_connect, Target_BASE, TABLE);



			if (LIST_COLUMNS.Count != LIST_VALUES.Count)
			{
				MessageBox.Show("Кол-во параметров и колонок таблицы не совпадает:" + LIST_COLUMNS.Count + " " + LIST_VALUES.Count);
				return null;
			}

			if (LIST_COLUMNS.Count == 0 || LIST_VALUES.Count == 0)
			{
				MessageBox.Show("Длина одного из листов == 0.");
				return null;
			}



			string SQL = "INSERT INTO " + TABLE + "(\n";


			for (int i = 0; i < LIST_COLUMNS.Count; i++)
			{

				SQL += LIST_COLUMNS[i];

				if (i != LIST_COLUMNS.Count - 1)
					SQL += ", \n";

			}



			SQL += "\n)VALUES(\n";

			for (int i = 0; i < LIST_VALUES.Count; i++)
			{

				SQL += "" + LIST_VALUES[i] + " ";

				if (i != LIST_VALUES.Count - 1)
					SQL += ", \n";

			}

			SQL += "\n); ";





			return SQL;
		}
		// ЗАКОНЧЕНО




		//########################################################################
		//########################################################################


		public static void UNIV_CONTAINER_FILLER(Control CONTAINER, string ELEMENT, List<string> TEXTS , EventHandler HANDLER = null , int[] PARAMS = null , bool NEED_CLEAN = true)
		{  // CONTAINER  ELEMENT  TEXTS  HANDLER_0  PARAMS_0  NEED_CLEAN_false



			int cb_first_top_indent = 25;
			int cb_left_indent = 15; //15; //отступ слева

			int cb_between_indent = 30; //между

			int size_width = 150;
			int size_heigth = 25;

			int prefix = 999;

			if (PARAMS != null && PARAMS[0] != 9999)// не пришли или берем дефолт, но без очистки
			{
				//UNIV_MSG_STR("ewwr");
				cb_first_top_indent = PARAMS[0];
				cb_left_indent = PARAMS[1]; //15; //отступ слева

				cb_between_indent = PARAMS[2]; //между

				size_width = PARAMS[3];
				size_heigth = PARAMS[4];
				prefix = PARAMS[5];
			}




			
			if (NEED_CLEAN)
			{
				CONTAINER.Controls.Clear();
			}
			
			//UNIV_MSG_STR("prefix="+ prefix);

			//TEXTS.Sort(); // робит



			int total_cnt_cboxes = TEXTS.Count;


			
			Control[] cb = new Control[total_cnt_cboxes];// all_names.Length ];


			for (int i = 0; i < total_cnt_cboxes; i++) //cb.Length; i++)
			{

				switch (ELEMENT)
				{
					case "RB":  cb[i] = new RadioButton(); break;
					case "CB":  cb[i] = new CheckBox();    break;
					case "TB":  cb[i] = new TextBox();     break;
					case "LBL": cb[i] = new Label();       break;
					case "GB":  cb[i] = new GroupBox();    break;
					case "BTN": cb[i] = new Button();      break;
				}


				
				//cb[i].Location = new System.Drawing.Point(15, 30 + i * 30);
				cb[i].Top  = cb_first_top_indent + ( i * cb_between_indent) ;
				cb[i].Left = cb_left_indent;

				cb[i].Name = "Control_"+ prefix + "_" + i.ToString();
				cb[i].Size = new System.Drawing.Size(size_width, size_heigth);

				if(HANDLER != null)
					cb[i].Click += HANDLER;

				//cb[i].TabIndex = i;

				if ( TEXTS[i] != null )
					cb[i].Text = TEXTS[i]; //i.ToString(); //all_names[i]+"ffffffffffffffffffffffffffffffffffffffffff" ;

				//cb[i].BringToFront();


				CONTAINER.Controls.Add(cb[i]);


			}
			
			

		}
		// ЗАКОНЧЕНО



		public static string UNIV_GET_TEXT_CHECKED_RB( Control groupbox_sended )
		{


			//перебор всех радио в боксе
			foreach (RadioButton RadioButt in groupbox_sended.Controls.OfType<RadioButton>())
			{
				if (RadioButt.Checked )
				{
					return RadioButt.Text;
				}
			}



			//RadioButton radioButton = (RadioButton)sender;
			//if (radioButton.Checked)
			//{
			//    MessageBox.Show("Вы выбрали " + radioButton.Text);
			//}

			return null;
		}
		// ЗАКОНЧЕНО


		//########################################################################
		//########################################################################





		//########################################################################
		//########################################################################


		// Много строк с 1 колонкой => 1 строка и много колонок
		public static List<string> UNIV_CONVERT_LIST_LIST_IN_ONE_LIST(List<List<string>> LIST)
		{
			// + проверка что только один столбец
			//foreach (List<string> val in LIST)
			//	if (val[1] != null)
			//	{
			//		return null;
			//	}
			
			List<string> sites = new List<string>();

			foreach (List<string> val in LIST)
				//if (sites.IndexOf(val[0]) == -1)//Еще нет в списке
					sites.Add(val[0]);

			return sites;
		}
		// ЗАКОНЧЕНО  дописать что if(val в массиве искл) то не пишем


		//########################################################################

		public static void UNIV_MSG_LIST( List<string> list, string Comment = "")
		{
			if (list is null) return;


			string MSG = "";

			for (int i = 0; i < list.Count; i++)
			{
				MSG += "[" + i + "] = \"" + list[i] + "\"\n";
			}

			MessageBox.Show(MSG, Comment);
		}
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ

		public static void UNIV_MSG_LIST_LIST(List<List<string>> list_list, string Comment = "")
		{

			if (list_list is null) return;

			for (int i = 0; i < list_list.Count; i++)
			{
				UNIV_MSG_LIST(list_list[i], i + 1 + " из " + list_list.Count + " " + Comment);
			}

		}
		// Базируется на UNIV_MSG_LIST
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ

		public static void UNIV_MSG_ARRAY(string[] array1, string Comment = "-", string[] array2 = null)
		{

			if (array1 == null) return;


			string MSG = "";

			int ar1_len = array1.Length;
			int ar2_len;

			//пришел ли 2 массив?
			if (array2 == null)
				ar2_len = -99;//так не пустит в цикл для двух
			else
				ar2_len = array2.Length;


			if (ar2_len >= 0 && ar1_len == ar2_len)//пришло 2 массива и они одинакового размера
			{
				for (int i = 0; i < array1.Length; i++)
				{
					MSG += "[" + i + "] = \"" + array1[i] + "\" = \"" + array2[i] + "\"\n";
				}
			}
			else
			{
				for (int i = 0; i < array1.Length; i++)
				{
					MSG += "[" + i + "] = \"" + array1[i] + "\"\n";
				}
			}


			MessageBox.Show(MSG, Comment);

		}
		//Переписать

		public static void UNIV_MSG_STR(string str, string Comment = "-")
		{
			MessageBox.Show(str, Comment);
		}
		// ЗАКОНЧЕНО

		//########################################################################


		public static void UNIV_ALL_EXCEPTION_INFO(Exception ex, string my_info = "-")
		{
			string MSG = "";

			MSG += "Мое описание: " + my_info + "\n\n";
			MSG += "Имя метода: " + ex.TargetSite + "\n\n";
			MSG += "Описание: " + ex.Message + "\n\n";
			MSG += "Source: " + ex.Source + "\n\n"; //ЮЗЛЕС  (почти всегда будет писать 'Интерфейс')
			MSG += "StackTrace: " + ex.StackTrace + "\n\n";

			MessageBox.Show(MSG, my_info);
		}
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ (больше нечего дописывать)


		// Окошко с кнопками
		public static bool UNIV_MSG_ASK(string MSG = "", string TITLE = "" , string btns = "YN" , string icon = "quest")
		{

			MessageBoxIcon View_Icon = new MessageBoxIcon();
			switch (icon)
			{
				case "aster": View_Icon = MessageBoxIcon.Asterisk;    break;
				case "error": View_Icon = MessageBoxIcon.Error;       break;
				case "exclam": View_Icon = MessageBoxIcon.Exclamation; break;
				case "hand": View_Icon = MessageBoxIcon.Hand;         break;
				case "info": View_Icon = MessageBoxIcon.Information;  break;
				case "none": View_Icon = MessageBoxIcon.None;         break;
				case "quest": View_Icon = MessageBoxIcon.Question;    break;
				case "stop": View_Icon = MessageBoxIcon.Stop;         break;
				case "warn": View_Icon = MessageBoxIcon.Warning;      break;
				default: View_Icon = MessageBoxIcon.None; break;
			}

			
			MessageBoxButtons Viev_Btns = new MessageBoxButtons();
			switch (btns)
			{
				case "ARI": Viev_Btns = MessageBoxButtons.AbortRetryIgnore; break;
				case "O": Viev_Btns = MessageBoxButtons.OK;          break;
				case "OC": Viev_Btns = MessageBoxButtons.OKCancel;    break;
				case "RC": Viev_Btns = MessageBoxButtons.RetryCancel; break;
				case "YN": Viev_Btns = MessageBoxButtons.YesNo;       break;
				case "YNC": Viev_Btns = MessageBoxButtons.YesNoCancel; break;
					
				default: Viev_Btns = MessageBoxButtons.OK; ; break;
			}


			DialogResult Result = MessageBox.Show(MSG, TITLE, Viev_Btns, View_Icon);

			switch (Result)
			{
				case DialogResult.OK:
				case DialogResult.Yes:
				case DialogResult.Retry:
										return true;

				case DialogResult.Abort:
				case DialogResult.Cancel:
				case DialogResult.No:
										return false;
					
				case DialogResult.Ignore:
				case DialogResult.None:
										return false;

				default: return false; // Юзлесс
			}

		}
		//ЗАКОНЧЕНО ПОЛНОСТЬЮ


		public static void UNIV_PRINT_PUBLIC_VARS( object targ )
		{
			// Выведет все компоненты и из значения
			//FieldInfo[] fi = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);


			//выведет все PUBLIC поля класса
			FieldInfo[] fi = targ.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			StringBuilder sb = new StringBuilder();


			foreach (FieldInfo f in fi)
				sb.AppendFormat("{0} = \"{1}\"\n", f.Name, f.GetValue(targ));
			//sb.AppendFormat("Имя: {0}; Значение: {1}\n", f.Name, f.GetValue(this));


			UNIV_MSG_STR(sb.ToString(), "Все PUBLIC поля класса." );

		}
		// ЗАКОНЧЕНО ПОЛНОСТЬЮ   Вызов: UNIV_PRINT_PUBLIC_VARS( this );


		public static string UNIV_Give_curr_datetime(string mode = "datetime")
		{
			string Curr_date = DateTime.Now.ToLongDateString();

			string Curr_time = DateTime.Now.ToLongTimeString(); // 8:49:10

			switch (mode)
			{
				case "date": return Curr_date;
				case "time": return Curr_time;
				case "datetime": return Curr_date + " " + Curr_time;

				default: return Curr_date + " " + Curr_time;
			}
		}


		//########################################################################
		//########################################################################


		/*
		   //Вроде ЗАКОНЧЕНО
        public void ADD_TO_LIST_NEW_PROCESS(string arg_DB_name = "mybase", string arg_Launch_type = "URL_GRABBER", List<string> mass_args = null)
        {

            if (path_filled == false)
            {
                MessageBox.Show("Не заполнены пути");
                return;
            }



            // true = НЕ показывать консоль с выводом echo
            bool see_php_console_echo = true;
            if (gl_DEBUG_show_phpexe_console_echo) // если видеть
                see_php_console_echo = false;



            ProcessStartInfo ProcInfo = new ProcessStartInfo(); // new ProcessStartInfo( @"L:\USR\php\php.exe", "spawn");

            // *** Постоянные настройки ***            
            //ProcInfo.WorkingDirectory = textBox2.Text;
            ProcInfo.CreateNoWindow = see_php_console_echo; //TRUE = НЕ создавать окно
            //ProcInfo.ErrorDialog = false;
            //ProcInfo.ErrorDialogParentHandle = IntPtr.Zero;
            ProcInfo.UseShellExecute = false; //false нужно ли использовать оболочку операционной системы для запуска процесса.
            ProcInfo.RedirectStandardOutput = false; //TRUE=НЕ ВЫВОДИТЬ В php консоль(будет пуста)
            //ProcInfo.WindowStyle = ProcessWindowStyle.Hidden; // пока хз




            ProcInfo.FileName = path_phpexe;// сделать приемник и что-нибудь с пробелами в пути



            //склейка массива в строку ARGUMENTS
            //Борьба с пробелами в аргументах
            string ARGUMENTS = " " +
                               DB_host + " " +
                               DB_name + " " +
                               DB_pass + " " +
                           arg_DB_name + " " +
                       arg_Launch_type;


            if (mass_args != null)
                foreach (string arg in mass_args)
                    ARGUMENTS += " " + arg; // клеим " арг арг арг"



            ProcInfo.Arguments = @"-f " + path_script + ARGUMENTS;
            //ProcInfo.Arguments = string.Format("{0} {1} {2}", @"""-f""", @"""L:\USR\www\!test_proc\123.php""", @"""TEXT"""); ;

            test_label2.Text += path_phpexe + @"-f" + path_script + ARGUMENTS + "\n";



            // Process.Start(ProcInfo); // ПОЛНЫЙ запуск
            Process myProcess = new Process();

            myProcess.StartInfo = ProcInfo;//запихнули все что выше как параметры процесса




            //ProcInfo.Close();
            //ProcInfo.Dispose();


            //myProcess.Start();



            LIST_ALL_created_proc.Add(myProcess);





            //label2.Text += myProcess.StandardOutput.ReadToEnd();//читает ВСЕ, но только в конце  //ЕСЛИ ProcInfo.RedirectStandardOutput = TRUE;



            /*
            StreamReader myStreamReader = myProcess.StandardOutput; //ЕСЛИ ProcInfo.RedirectStandardOutput = TRUE;
            richTextBox1.Text += ProcInfo.StandardOutput.ReadToEnd();

            //Стрим не обязателен
           
   


            //ProcInfo.Close();
            //ProcInfo.Dispose();

            // 



	}
	// Пишет процессы в лист LIST_ALL_created_proc




	//################# Процессы ###################
	//################# DGV большие ###################


	//вроде ГОТОВО
	private void print_query_result_in_DGV(string DB_name, string SQL_query, DataGridView Target_DGV)
	{

		//очистка DGV
		Target_DGV.Rows.Clear();
		Target_DGV.Columns.Clear();



		DataTable dt = UNIV_GET_QUERY_RESULT(SQL_query, DB_name);

		if (dt == null) return;


		DataRow[] myData = dt.Select();//было



		//ФУЛЛ ГОТОВО = создать ВСЕ колонки (сколько надо)
		foreach (DataColumn one_column in dt.Columns)
			Target_DGV.Columns.Add(one_column.ColumnName, one_column.ColumnName);




		//создать ВСЕ строки (сколько надо)
		for (int i = 0; i < myData.Length; i++)//myData.Length
			Target_DGV.Rows.Add();




		for (int i = 0; i < myData.Length; i++)//перебор по СТРОКАМ
		{
			for (int j = 0; j < myData[i].ItemArray.Length; j++)//Перебор по ЯЧЕЙКАМ КАЖДОЙ отдельной строки
			{
				Target_DGV.Rows[i].Cells[j].Value = myData[i].ItemArray[j];
			}
		}



	}



	//################# DGV большие ###################
	//################# DGV мелкие  ###################

	public void UNIV_DGV_clean(DataGridView Target_DGV, string type = "Rows Columns AnyString")
	{
		//DataGridView Target_DGV = (DataGridView)sender;

		switch (type)
		{
			case "Rows": Target_DGV.Rows.Clear(); break;
			case "Columns": Target_DGV.Columns.Clear(); break;
			default:
				Target_DGV.Rows.Clear();
				Target_DGV.Columns.Clear();
				break;
		}


	}//Чистим ВЕСЬ DGV
	 // цепляем к нажатию кнопки   UNIV_DGV_clean( цель ,"AnyString")

	public void UNIV_DGV_Write_nums_rows(object sender, DataGridViewRowsAddedEventArgs e)
	{
		DataGridView Target_DGV = (DataGridView)sender;

		Target_DGV.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
	}//дописываем номера строк





		 */


		/*
		  //Добавка groupbox и чекбоксов на панель
        private void add_button_add_print_all_db_Click(object sender, EventArgs e)
        {
            if (DB_connected == false)
            {
                MessageBox.Show("Не подключен к БД");
                return;
            }



            add_panel_checkboxes.Controls.Clear();



            int cnt_cb_in_one_groupbox = Convert.ToInt32(add_textBox_cnt_in_col.Text);

            string[] all_names = GET_ALL_SERVER_BASE_NAMES();





            int total_cnt_gboxes = Convert.ToInt32(all_names.Length / cnt_cb_in_one_groupbox) + 1;

            //int total_cnt_cboxes = all_names.Length;
            int total_cnt_cboxes = all_names.Length;




            int gb_indent = 15; // расстояние между гбоксами




            int top_indent_for_buttons = 140; // отступ от верха(для кнопок)
            int between_cb_indent = 30; //между чекбоксами

            // Высота гбокса
            int groupboxes_width = top_indent_for_buttons + (cnt_cb_in_one_groupbox * between_cb_indent) + 25;


            GroupBox[] gb = new GroupBox[total_cnt_gboxes];// total_cnt_gboxes ];


            for (int g = 0; g < total_cnt_gboxes; g++)
            {

                gb[g] = new System.Windows.Forms.GroupBox();

                gb[g].Name = "add_panel_GroupBox_" + g.ToString();
                gb[g].Size = new System.Drawing.Size(205, groupboxes_width); //200 530

                gb[g].Top = 5;
                gb[g].Left = gb_indent;


                gb[g].Text = (g * cnt_cb_in_one_groupbox).ToString() + "-" + ((g + 1) * cnt_cb_in_one_groupbox - 1).ToString();


                add_panel_checkboxes.Controls.Add(gb[g]);


                gb_indent += gb[g].Width + 15;








            }




            Button[] b = new Button[5];

            b[0] = new System.Windows.Forms.Button();
            b[0].Name = "add_panel_Button_gb_laun";
            b[0].Size = new System.Drawing.Size(65, 25); //200 530
            b[0].Top = 20;
            b[0].Left = 5;
            b[0].Text = "Запуск";
            //b[0].Click += new System.EventHandler(button1_Click);


            b[1] = new System.Windows.Forms.Button();
            b[1].Name = "add_panel_Button_gb_pause";
            b[1].Size = new System.Drawing.Size(65, 25); //200 530
            b[1].Top = 20;
            b[1].Left = 70;
            b[1].Text = "Пауза";
            //b[1].Click += new System.EventHandler(button1_Click);

            b[2] = new System.Windows.Forms.Button();
            b[2].Name = "add_panel_Button_gb_stop";
            b[2].Size = new System.Drawing.Size(65, 25); //200 530
            b[2].Top = 20;
            b[2].Left = 135;
            b[2].Text = "Стоп";
            //b[2].Click += new System.EventHandler(button1_Click);

            b[3] = new System.Windows.Forms.Button();
            b[3].Name = "add_panel_Button_gb_set_unset_all";
            b[3].Size = new System.Drawing.Size(65, 25); //200 530
            b[3].Top = 47;
            b[3].Left = 5;
            b[3].Text = "Все";
            b[3].Click += new System.EventHandler(hand_univ_gbox_set_unset_all_combobox);

            b[4] = new System.Windows.Forms.Button();
            b[4].Name = "add_panel_Button_gb_invert";
            b[4].Size = new System.Drawing.Size(65, 25); //200 530
            b[4].Top = 47;
            b[4].Left = 135;
            b[4].Text = "Инверт";
            b[4].Click += new System.EventHandler(hand_univ_gbox_invert_all_combobox);

            



            // Проблема в схожести
            int k = 0;
            foreach (Control control in add_panel_checkboxes.Controls.OfType<GroupBox>())
            {
                for (int i = 0; i < 5; i++)
                {
                    Button buf = b[i];

                    buf.Name += "_" + k.ToString() + "_" + i.ToString();

                    //gb[g]

                    control.Controls.Add( buf );

                    //tabPage_test.Controls.Add(b[i]);

                }



                k += 1;
            }






            CheckBox[] cb = new CheckBox[total_cnt_cboxes];// all_names.Length ];

            //int next_draw_cb_id = 0;



            int targen_gbox_id = 0; // в какой бокс пишем
            int last_cb_top_indent = 0; // на сколько сдвинуться вниз от последнего(обнуляется в новом боксе)


            for (int i = 0; i < total_cnt_cboxes; i++) //cb.Length; i++)
            {

                if (i % cnt_cb_in_one_groupbox == 0 && i != 0)
                {
                    last_cb_top_indent = 0;
                    targen_gbox_id += 1;
                }



                cb[i] = new System.Windows.Forms.CheckBox();


                //cb[i].Location = new System.Drawing.Point(15, 30 + i * 30);
                cb[i].Top = top_indent_for_buttons + last_cb_top_indent * between_cb_indent;
                cb[i].Left = 15;



                cb[i].Name = "add_panel_CheckBox_" + i.ToString();
                cb[i].Size = new System.Drawing.Size(150, 23);

                cb[i].TabIndex = i;
                cb[i].Text = all_names[i]; //i.ToString(); //all_names[i]+"ffffffffffffffffffffffffffffffffffffffffff" ;

                //cb[i].BringToFront();



                last_cb_top_indent += 1;

                gb[targen_gbox_id].Controls.Add(cb[i]);


            }


            add_button_launch_all_checked.Enabled = true;
            add_button_launch_all        .Enabled = true;
            add_button_all_set_or_unset  .Enabled = true;
            add_button_all_invert        .Enabled = true;
        }



        // set/unset все чекбоксы
        // ЗАКОНЧЕНО
        // Мини-костыли
        private void add_button_all_set_or_unset_Click(object sender, EventArgs e)
        {

            //лучше оставить как есть сейчас, а не пихать в ту функцию(т к тут обреботка всего и вся сразу, а там по отдельности)


            bool curr_state = true;

            foreach (GroupBox group_control in add_panel_checkboxes.Controls.OfType<GroupBox>())
            {
                foreach (CheckBox control in group_control.Controls.OfType<CheckBox>())
                {
                    if (control.Checked == false)
                    {
                        curr_state = false;
                        break;
                    }
                }
            }

            if (curr_state == false)
            {
                foreach (GroupBox group_control in add_panel_checkboxes.Controls.OfType<GroupBox>())
                {
                    foreach (CheckBox control in group_control.Controls.OfType<CheckBox>())
                    {
                        control.Checked = true;
                    }
                }
            }
            else
            {
                foreach (GroupBox group_control in add_panel_checkboxes.Controls.OfType<GroupBox>())
                {
                    foreach (CheckBox control in group_control.Controls.OfType<CheckBox>())
                    {
                        control.Checked = false;
                    }
                }

            }
        }


        // инвертировать выбор
        // ЗАКОНЧЕНО
        private void add_button_all_invert_Click(object sender, EventArgs e)
        {

            foreach (GroupBox group_control in add_panel_checkboxes.Controls.OfType<GroupBox>())
            {
                univ_gbox_invert_all_combobox(group_control);
            }

        }


  

        //#################################
        //########## Кнопки из гбоксов



        //ЗАКОНЧЕНО
        //хендлер для кнопок в гбоксе
        public void hand_univ_gbox_invert_all_combobox(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Control gb1 = btn.Parent;
            GroupBox gb = (GroupBox)gb1;

            // тут мы имеем гбокс в котором нажата кнопка
            univ_gbox_invert_all_combobox(gb);


        }

        //ЗАКОНЧЕНО
        //хендлер для кнопок в гбоксе
        public void hand_univ_gbox_set_unset_all_combobox(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Control gb1 = btn.Parent;
            GroupBox gb = (GroupBox)gb1;

            // тут мы имеем гбокс в котором нажата кнопка
            univ_gbox_set_unset_all_combobox(gb);


        }


        //#################################
        //########## мини функции

        //ЗАКОНЧЕНО
        // инвертирует выбор для всех чбоксов в гбоксе
        public void univ_gbox_invert_all_combobox(GroupBox gbox)
        {


            foreach (CheckBox control in gbox.Controls.OfType<CheckBox>())
            {
                if (control.Checked == false)
                {
                    control.Checked = true;

                }
                else
                {
                    control.Checked = false;
                }
            }


        }


        //set/unset всего гбокса
        public void univ_gbox_set_unset_all_combobox(GroupBox gbox)
        {

            bool curr_state = true;


            foreach (CheckBox control in gbox.Controls.OfType<CheckBox>())
            {
                if (control.Checked == false)
                {
                    curr_state = false;
                    break;
                }
            }


            if (curr_state == false)
            {
                foreach (CheckBox control in gbox.Controls.OfType<CheckBox>())
                {
                    control.Checked = true;
                }
            }
            else
            {
                foreach (CheckBox control in gbox.Controls.OfType<CheckBox>())
                {
                    control.Checked = false;
                }
            }

        }
		 
		 */









		//##########################



		/*
		
		
		//вроде ГОТОВО
		private void print_query_result_in_DGV(string DB_name, string SQL_query, DataGridView Target_DGV)
		{

			//очистка DGV
			Target_DGV.Rows.Clear();
			Target_DGV.Columns.Clear();







			DataTable dt = UNIV_GET_QUERY_RESULT_DT(SQL_query, DB_name);








			if (dt == null) return;


			DataRow[] ALL_query_rows = dt.Select();//было



			//ФУЛЛ ГОТОВО = создать ВСЕ колонки (сколько надо)
			foreach (DataColumn one_column in dt.Columns)
				Target_DGV.Columns.Add(one_column.ColumnName, one_column.ColumnName);




			//создать ВСЕ строки (сколько надо)
			for (int i = 0; i < ALL_query_rows.Length; i++)//ALL_query_rows.Length
				Target_DGV.Rows.Add();




			for (int i = 0; i < ALL_query_rows.Length; i++)//перебор по СТРОКАМ
			{
				for (int j = 0; j < ALL_query_rows[i].ItemArray.Length; j++)//Перебор по ЯЧЕЙКАМ КАЖДОЙ отдельной строки
				{
					Target_DGV.Rows[i].Cells[j].Value = ALL_query_rows[i].ItemArray[j];
				}
			}



		}

	




		//########### DGV большие
		//########################################################
		//##########################################################
		//########################################################
		//########### DGV мелкие

		public void UNIV_DGV_clean(DataGridView Target_DGV, string type = "Rows Columns AnyString")
		{
			//DataGridView Target_DGV = (DataGridView)sender;

			switch (type)
			{
				case "Rows": Target_DGV.Rows.Clear(); break;
				case "Columns": Target_DGV.Columns.Clear(); break;
				default:
					Target_DGV.Rows.Clear();
					Target_DGV.Columns.Clear();
					break;
			}


		}//Чистим ВЕСЬ DGV
		 // цепляем к нажатию кнопки   UNIV_DGV_clean( цель ,"AnyString")

		public void UNIV_DGV_Write_nums_rows(object sender, DataGridViewRowsAddedEventArgs e)
		{
			DataGridView Target_DGV = (DataGridView)sender;

			Target_DGV.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
		}//дописываем номера строк




		*/









		/*
	   foreach ( Control control in add_panel_checkboxes.Controls.OfType<GroupBox>())
	   {
			add_panel_checkboxes.Controls.Remove(control);
			add_panel_checkboxes.Controls.Clear();
	   }
	   */


		//for ( int i = 0 ; i < 1 ; i++ )//

		//Random random = new Random();
		//int randomNumber = random.Next(0, 100);


		//########################################################################
		//########################################################################

	} //### END CLASS

}//### END NAMESPACCE

//#######################
//### Свалка













//#######################