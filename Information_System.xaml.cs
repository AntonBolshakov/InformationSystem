using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using RegressionModels;
using System.Diagnostics;


namespace Regression_models
{
    public partial class Information_System_For_Regression_Analysis : Window
    {
        private int HeightScreen, WidthScreen, Col = 2, Row = 5, KeyBottomMenu = 1, KeyLoadFile = 1, KeySaveFile = 1, KeyDeleteET = 1, IndicatorPrediction = 1, NumberDigits;
        private string TypeWindow, TypeContent = "Table", TypeContentResult = "Result", ErrorString = "", TypeContentPrediction = "InputFactors", Info;
        private string[] default_files_name, ElementsList, _Report, TextForecast;
        private double[,] Array;
        private double[] Coefficients, Factors;
        private bool key = false, EditingTableON = false, EditingTableOpen = false, KeyStartedT1 = true, KeyStartedT2 = true, ErrorUpdate = false, ErrorUpdateFactors = false, AutoSave;
        private Exception _Error;
        private double Det, Smoothing_parameter, CoefX = 1;
        private Dictionary<string, double> DataEstimation;
        private Dictionary<string, string> InfoModel;
        private Dictionary<string, double> Forecast;

        private DataSet DS;
        private RegressionModel RM;
        private RegressionEstimation RE;

#region Инициализация объектов окна Information_System_For_Regression_Analysis

        private Grid BlockMenu, BlockContent, BlockForContent, BlockForTable, BlockForChart, BlockForResult, BlockForSaveResult, BlockForElementsList;
        private Grid GridForButton, BlockMenuInfo, BlockContentInfo, BlockForButtons, MainEditingTable, EditingTable, BlockForInputFactors, BlockForPrediction;
        private Grid[] Table_Row;
        private ListBox DataTable;
        private ColumnDefinition[] Table_Column;
        private RowDefinition[,] Rows;
        private ColumnDefinition[,] Column;
        private Label TranslucentBlock, LabelHeader, LabelHeaderReport, LabelSubHeader, Lbl_AmountRows, Lbl_AmountColumns, LabelHeaderSaveResult, LabelHBM_LSM, Border_BM_LSM, LabelHBM_FBM, Border_BM_FBM;
        private Label TBox_Header_EditingTable, TBox_Add_Field, TBox_Delete_Field, Fon_Add_Field, Fon_Delete_Field, FonForTCMenu, MinValueSlider, MaxValueSlider, HeaderPrediction, SubHeaderPrediction, HeaderResultPrediction;
        private TextBox Text, TextReport, Txt_AmountRows, Txt_AmountColumns, ValueSlider, TextPrediction;
        private TextBox[] TextBox_FilesName, TextBox_Elements_List, TextBox_Prediction, Label_Factors, Label_AVF;
        private Label[] Label_Elements_List;
        private CheckBox[] CheckBox_Elements_List;
        private TextBox[,] DataTable_Cell;
        private Button BackToProgram, HelpButton, Button_Load, Button_Save, Button_EditingTable, Button_SaveChart, Button_Start, Button_Add_Field, Button_Delete_Field;
        private Button Button_Table, Button_Chart, Button_Result, SavingResults, SaveResults, Button_ResultTable, Button_ResultChart, Button_Home, MainButton_Prediction, Button_Prediction, Button_Input_DS_Pred, Button_SavePrediction, Button_FolderSelection;
        private ComboBox LoadFile, SaveFile, DeleteET;
        private ComboBoxItem LoadFileTxt, LoadFileXml, SaveFileTxt, SaveFileXml, DeleteET1, DeleteET2;
        private TabControl TC_BottomMenu;
        private TabItem TI_DataTable, TI_LoadingData, TI_SaveData;
        private RadioButton LinearModel, SemilogModel, ParabolicModel, DegreesModel, AdaptiveModel, BrownModel, FindingBestModel;
        private ScrollViewer ScrollSaveResult, ScrollChart, ScrollPrediction;
        private Canvas _Chart;
        private Slider _slider;

#endregion

#region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public Information_System_For_Regression_Analysis()
        {
            try
            {
                StreamReader SizeScreen = new StreamReader("Options/SizeScreen.txt");
                WidthScreen = Convert.ToInt32(SizeScreen.ReadLine());
                HeightScreen = Convert.ToInt32(SizeScreen.ReadLine());
                SizeScreen.Close();
            }
            catch
            {
                WidthScreen = 930;
                HeightScreen = 630;
            }

            try
            {
                StreamReader Options = new StreamReader("Options/Options.txt");
                TypeWindow = Options.ReadLine();
                AutoSave = Convert.ToBoolean(Options.ReadLine());
                Options.Close();
                if (TypeWindow != "Normal" && TypeWindow != "Maximized") TypeWindow = "Normal";
            }
            catch
            {
                TypeWindow = "Normal";
                AutoSave = true;
            }

            InitializeComponent();

            default_files_name = new string[7];
            default_files_name[0] = "New folder";
            default_files_name[1] = "New file.txt";
            default_files_name[2] = "New file.xml";
            default_files_name[3] = "Chart data set.png";
            default_files_name[4] = "Chart model.png";
            default_files_name[5] = "Report.txt";
            default_files_name[6] = "Forecast.txt";

            ElementsList = new string[8];
            ElementsList[0] = "Имя папки:";
            ElementsList[1] = "Сохранить всё";
            ElementsList[2] = "Файл (.txt), содержащий исходные данные";
            ElementsList[3] = "Файл (.xml), содержащий исходные данные";
            ElementsList[4] = "Изображение (.png), содержащие график на иснове исходных данных";
            ElementsList[5] = "Изображение (.png), содержащие график на иснове исходных данных и построенной модели";
            ElementsList[6] = "Файл (.txt), содержащий отчёт о построенной модели";
            ElementsList[7] = "Файл (.txt), содержащий прогноз по построенной модели";

            if (TypeWindow == "Normal") RegressionModels.Style = (Style)this.Resources["WindowStyle1"];
            else if (TypeWindow == "Maximized") RegressionModels.Style = (Style)this.Resources["WindowStyle2"];
            AutoSaveSizeScreen.IsChecked = AutoSave;
            MenuItemScreen();
            key = true; RegressionModels.Height = HeightScreen; key = false;
            RegressionModels.Width = WidthScreen;
            MenuItemTypeView();

            DS = new DataSet();
            RM = new RegressionModel();
            RE = new RegressionEstimation();
            NumberDigits = 2;
            Smoothing_parameter = 0.29;

            Template_1();
        }

#endregion



#region Шаблон №1

        /// <summary>
        /// Инициализация элементов шаблона №1
        /// </summary>
        private void Template1_Elements()
        {
            Column = new ColumnDefinition[10, 10];
            Rows = new RowDefinition[15, 22];

            #region Элементы
                #region Пространство под меню, располагающееся слева
                    BlockMenu = new Grid();

                    // Задание новой разметки строк и столбцов для BlockMenu
                    for (int i = 0; i < 21; i++)
                    {
                        Rows[0, i] = new RowDefinition();
                        BlockMenu.RowDefinitions.Add(Rows[0, i]);
                        if (i < 17 && i % 2 != 0) Rows[0, i].Height = new GridLength(20);
                        else if (i < 17 && i % 2 == 0) Rows[0, i].Height = new GridLength(20);
                    }

                    Rows[0, 0].Height = new GridLength(2);
                    Rows[0, 1].Height = new GridLength(35);
                    Rows[0, 2].Height = new GridLength(20);
                    Rows[0, 14].Height = new GridLength(30);
                    Rows[0, 15].Height = new GridLength(30);
                    Rows[0, 16].Height = new GridLength(35);
                    Rows[0, 17].Height = new GridLength(15);
                    Rows[0, 18].Height = new GridLength(35);
                    Rows[0, 20].Height = new GridLength(2);

                    BlockMenu.UpdateLayout();

                    // Фон
                    TranslucentBlock = new Label();
                    TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                    BlockMenu.Children.Add(TranslucentBlock);
                    Grid.SetColumn(TranslucentBlock, 0);
                    Grid.SetRow(TranslucentBlock, 0);
                    Grid.SetRowSpan(TranslucentBlock, 22);

                    // Контент для BlockMenu

                    Border_BM_LSM = new Label();
                    Border_BM_LSM.Style = (Style)this.Resources["BorderBM"];
                    Border_BM_LSM.ToolTip = "Построение моделей с использованием метода наименьших квадратов.";
                    BlockMenu.Children.Add(Border_BM_LSM);
                    Grid.SetColumn(Border_BM_LSM, 0);
                    Grid.SetRow(Border_BM_LSM, 1);
                    Grid.SetRowSpan(Border_BM_LSM, 14);

                    LabelHBM_LSM = new Label();
                    LabelHBM_LSM.Content = "Выбор модели";
                    LabelHBM_LSM.Style = (Style)this.Resources["Label_BlockMenu"];
                    LabelHBM_LSM.ToolTip = "Построение моделей с использованием метода наименьших квадратов.";
                    BlockMenu.Children.Add(LabelHBM_LSM);
                    Grid.SetColumn(LabelHBM_LSM, 0);
                    Grid.SetRow(LabelHBM_LSM, 1);
                    
                    LinearModel = new RadioButton();
                    LinearModel.Content = "Линейная";
                    LinearModel.ToolTip = "Построение линейной модели.";
                    LinearModel.IsChecked = true;
                    LinearModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(LinearModel);
                    Grid.SetColumn(LinearModel, 0);
                    Grid.SetRow(LinearModel, 3);

                    SemilogModel = new RadioButton();
                    SemilogModel.Content = "Полулогарифмическая";
                    SemilogModel.ToolTip = "Построение полулогарифмической модели.";
                    SemilogModel.IsChecked = false;
                    SemilogModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(SemilogModel);
                    Grid.SetColumn(SemilogModel, 0);
                    Grid.SetRow(SemilogModel, 5);

                    ParabolicModel = new RadioButton();
                    ParabolicModel.Content = "Параболическая";
                    ParabolicModel.ToolTip = "Построение параболической модели.";
                    ParabolicModel.IsChecked = false;
                    ParabolicModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(ParabolicModel);
                    Grid.SetColumn(ParabolicModel, 0);
                    Grid.SetRow(ParabolicModel, 7);

                    DegreesModel = new RadioButton();
                    DegreesModel.Content = "Степенная";
                    DegreesModel.ToolTip = "Построение степенной модели.";
                    DegreesModel.IsChecked = false;
                    DegreesModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(DegreesModel);
                    Grid.SetColumn(DegreesModel, 0);
                    Grid.SetRow(DegreesModel, 9);

                    AdaptiveModel = new RadioButton();
                    AdaptiveModel.Content = "Адаптивная";
                    AdaptiveModel.ToolTip = "Определение наиболее подходящей модели\nс использованием метода конечных разностей.";
                    AdaptiveModel.IsChecked = false;
                    AdaptiveModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(AdaptiveModel);
                    Grid.SetColumn(AdaptiveModel, 0);
                    Grid.SetRow(AdaptiveModel, 11);

                    BrownModel = new RadioButton();
                    BrownModel.Content = "Модель Брауна";
                    BrownModel.ToolTip = "Определение наиболее подходящей модели\nс использованием модели Брауна.";
                    BrownModel.IsChecked = false;
                    BrownModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(BrownModel);
                    Grid.SetColumn(BrownModel, 0);
                    Grid.SetRow(BrownModel, 13);

                    Border_BM_FBM = new Label();
                    Border_BM_FBM.Style = (Style)this.Resources["BorderBM"];
                    Border_BM_FBM.ToolTip = "Поиск наилучшей модели для текущего набора данных.";
                    BlockMenu.Children.Add(Border_BM_FBM);
                    Grid.SetColumn(Border_BM_FBM, 0);
                    Grid.SetRow(Border_BM_FBM, 16);
                    Grid.SetRowSpan(Border_BM_FBM, 4);

                    LabelHBM_FBM = new Label();
                    LabelHBM_FBM.Content = "Поиск наилучшей модели";
                    LabelHBM_FBM.Style = (Style)this.Resources["Label_BlockMenu"];
                    LabelHBM_FBM.ToolTip = "Поиск наилучшей модели для текущего набора данных.";
                    BlockMenu.Children.Add(LabelHBM_FBM);
                    Grid.SetColumn(LabelHBM_FBM, 0);
                    Grid.SetRow(LabelHBM_FBM, 16);

                    FindingBestModel = new RadioButton();
                    FindingBestModel.Content = "Найти модель";
                    FindingBestModel.ToolTip = "Определение наиболее подходящей модели\nдля текущего набора данных.";
                    FindingBestModel.IsChecked = false;
                    FindingBestModel.Style = (Style)this.Resources["RadioButton_Style"];
                    BlockMenu.Children.Add(FindingBestModel);
                    Grid.SetColumn(FindingBestModel, 0);
                    Grid.SetRow(FindingBestModel, 18);

                #endregion
                #region Пространство для данных и работы с ними
                    BlockContent = new Grid();

                    // Задание новой разметки строк и столбцов для BlockContent
                    Rows[1, 0] = new RowDefinition();
                    Rows[1, 1] = new RowDefinition();
                    Rows[1, 1].Height = new GridLength(4);
                    Rows[1, 2] = new RowDefinition();
                    Rows[1, 2].Height = new GridLength(152);

                    // Разметка BlockContent
                    BlockContent.RowDefinitions.Add(Rows[1, 0]);
                    BlockContent.RowDefinitions.Add(Rows[1, 1]);
                    BlockContent.RowDefinitions.Add(Rows[1, 2]);

                    // Пространство для таблицы, графика и отчёта
                    BlockForContent = new Grid();

                    BlockContent.Children.Add(BlockForContent);
                    Grid.SetColumn(BlockForContent, 0);
                    Grid.SetRow(BlockForContent, 0);

                    // Пространство для таблицы
                    BlockForTable = new Grid();

                    // Пространство для графика
                    BlockForChart = new Grid();

                    // Пространство для результатов
                    BlockForResult = new Grid();

                    // Пространство для сохранения результатов
                    BlockForSaveResult = new Grid();

                    // Пространство для ввода значений факторов для прогнозирования
                    BlockForInputFactors = new Grid();

                    // Пространство для прогнозирования
                    BlockForPrediction = new Grid();
                #endregion
                #region Элементы при работе с таблицей
                    // Задание новой разметки строк и столбцов для BlockForTable
                    Column[1, 0] = new ColumnDefinition();
                    Column[1, 1] = new ColumnDefinition();
                    Column[1, 2] = new ColumnDefinition();
                    Column[1, 1].Width = new GridLength(0);
                    Column[1, 2].Width = new GridLength(0);

                    BlockForTable.ColumnDefinitions.Add(Column[1, 0]);
                    BlockForTable.ColumnDefinitions.Add(Column[1, 1]);
                    BlockForTable.ColumnDefinitions.Add(Column[1, 2]);

                    // Таблица для набора данных
                    DataTable = new ListBox();
                    DataTable.Style = (Style)this.Resources["ListBox_For_DataTable"];
                    DataTable.Background = new SolidColorBrush(Colors.Transparent);

                    // Фон
                    TranslucentBlock = new Label();
                    TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                    BlockForTable.Children.Add(TranslucentBlock);
                    Grid.SetColumn(TranslucentBlock, 0);
                    Grid.SetRow(TranslucentBlock, 0);

                    BlockForTable.Children.Add(DataTable);
                    Grid.SetColumn(DataTable, 0);
                    Grid.SetRow(DataTable, 0);

                    MainEditingTable = new Grid();

                    BlockForTable.Children.Add(MainEditingTable);
                    Grid.SetColumn(MainEditingTable, 2);
                    Grid.SetRow(MainEditingTable, 0);
                #endregion
                #region Элементы меню редактирования таблицы
                    // Пространство для EditingTable
                    EditingTable = new Grid();

                    // Задание новой разметки строк и столбцов для EditingTable
                    for (int i = 0; i < 12; i++)
                    {
                        Rows[2, i] = new RowDefinition();
                        EditingTable.RowDefinitions.Add(Rows[2, i]);
                    }

                    Rows[2, 0].Height = new GridLength(30);
                    Rows[2, 1].Height = new GridLength(20);
                    Rows[2, 2].Height = new GridLength(30);
                    Rows[2, 3].Height = new GridLength(15);
                    Rows[2, 4].Height = new GridLength(25);
                    Rows[2, 5].Height = new GridLength(25);
                    Rows[2, 6].Height = new GridLength(60);
                    Rows[2, 7].Height = new GridLength(20);
                    Rows[2, 8].Height = new GridLength(30);
                    Rows[2, 9].Height = new GridLength(15);
                    Rows[2, 10].Height = new GridLength(25);
                    Rows[2, 11].Height = new GridLength(60);
                    EditingTable.UpdateLayout();

                    // Элементы EditingTable
                    TBox_Header_EditingTable = new Label();
                    TBox_Header_EditingTable.Style = (Style)this.Resources["HeaderET"];
                    TBox_Header_EditingTable.Content = "Редактирование таблицы";

                    Fon_Add_Field = new Label();
                    Fon_Add_Field.Style = (Style)this.Resources["BorderET"];

                    TBox_Add_Field = new Label();
                    TBox_Add_Field.Style = (Style)this.Resources["SubHeaderET"];
                    TBox_Add_Field.Content = "Размер таблицы";

                    Lbl_AmountColumns = new Label();
                    Lbl_AmountColumns.Content = "Кол-во столбцов:";
                    Lbl_AmountColumns.Style = (Style)this.Resources["Label_ET"];

                    Txt_AmountColumns = new TextBox();
                    Txt_AmountColumns.PreviewTextInput += new TextCompositionEventHandler(TextChanged_IntegerNumber);
                    Txt_AmountColumns.PreviewKeyDown += new KeyEventHandler(TextBox_PreviewKeyDown);
                    Txt_AmountColumns.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteOrCutCommand));
                    Txt_AmountColumns.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnPasteOrCutCommand));
                    Txt_AmountColumns.Style = (Style)this.Resources["TextBox_ET"];

                    Lbl_AmountRows = new Label();
                    Lbl_AmountRows.Content = "Кол-во строк:";
                    Lbl_AmountRows.Style = (Style)this.Resources["Label_ET"];

                    Txt_AmountRows = new TextBox();
                    Txt_AmountRows.PreviewTextInput += new TextCompositionEventHandler(TextChanged_IntegerNumber);
                    Txt_AmountRows.PreviewKeyDown += new KeyEventHandler(TextBox_PreviewKeyDown);
                    Txt_AmountRows.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteOrCutCommand));
                    Txt_AmountRows.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnPasteOrCutCommand));
                    Txt_AmountRows.Style = (Style)this.Resources["TextBox_ET"];

                    Button_Add_Field = new Button();
                    Button_Add_Field.Name = "Button_Add_Field";
                    Button_Add_Field.Content = "Изменить";
                    Button_Add_Field.Style = (Style)this.Resources["Button_Editing_Table"];
                    Button_Add_Field.Click += new RoutedEventHandler(CreateTable_Click);

                    Fon_Delete_Field = new Label();
                    Fon_Delete_Field.Style = (Style)this.Resources["BorderET"];

                    TBox_Delete_Field = new Label();
                    TBox_Delete_Field.Style = (Style)this.Resources["SubHeaderET"];
                    TBox_Delete_Field.Content = "Удаление";

                    DeleteET = new ComboBox();
                    DeleteET.Name = "DeleteET";
                    DeleteET.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged_ComboBox);
                    DeleteET.Style = (Style)this.Resources["ComboBoxET"];
                    DeleteET1 = new ComboBoxItem();
                    DeleteET1.Content = "Удалить данные";
                    DeleteET2 = new ComboBoxItem();
                    DeleteET2.Content = "Удалить таблицу";

                    Button_Delete_Field = new Button();
                    Button_Delete_Field.Name = "Button_Delete_Field";
                    Button_Delete_Field.Content = "Удалить";
                    Button_Delete_Field.Click += new RoutedEventHandler(Delete_Click);
                    Button_Delete_Field.Style = (Style)this.Resources["Button_Editing_Table"];

                    // Фон для меню
                    TranslucentBlock = new Label();
                    TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                    MainEditingTable.Children.Add(TranslucentBlock);
                    Grid.SetColumn(TranslucentBlock, 0);
                    Grid.SetRow(TranslucentBlock, 0);

                    // Добавление элемента EditingTable в BlockForTable
                    MainEditingTable.Children.Add(EditingTable);
                    Grid.SetColumn(EditingTable, 0);
                    Grid.SetRow(EditingTable, 0);

                    // Добавление элементов в EditingTable
                    EditingTable.Children.Add(TBox_Header_EditingTable);
                    Grid.SetColumn(TBox_Header_EditingTable, 0);
                    Grid.SetRow(TBox_Header_EditingTable, 0);

                    EditingTable.Children.Add(Fon_Add_Field);
                    Grid.SetColumn(Fon_Add_Field, 0);
                    Grid.SetRow(Fon_Add_Field, 2);
                    Grid.SetRowSpan(Fon_Add_Field, 5);

                    EditingTable.Children.Add(TBox_Add_Field);
                    Grid.SetColumn(TBox_Add_Field, 0);
                    Grid.SetRow(TBox_Add_Field, 2);

                    EditingTable.Children.Add(Lbl_AmountColumns);
                    Grid.SetColumn(Lbl_AmountColumns, 0);
                    Grid.SetRow(Lbl_AmountColumns, 4);

                    EditingTable.Children.Add(Txt_AmountColumns);
                    Grid.SetColumn(Txt_AmountColumns, 1);
                    Grid.SetRow(Txt_AmountColumns, 4);

                    EditingTable.Children.Add(Lbl_AmountRows);
                    Grid.SetColumn(Lbl_AmountRows, 0);
                    Grid.SetRow(Lbl_AmountRows, 5);

                    EditingTable.Children.Add(Txt_AmountRows);
                    Grid.SetColumn(Txt_AmountRows, 1);
                    Grid.SetRow(Txt_AmountRows, 5);

                    Txt_AmountRows.Text = Convert.ToString(Row);
                    Txt_AmountColumns.Text = Convert.ToString(Col);

                    EditingTable.Children.Add(Button_Add_Field);
                    Grid.SetColumn(Button_Add_Field, 0);
                    Grid.SetRow(Button_Add_Field, 6);

                    EditingTable.Children.Add(Fon_Delete_Field);
                    Grid.SetColumn(Fon_Delete_Field, 0);
                    Grid.SetRow(Fon_Delete_Field, 8);
                    Grid.SetRowSpan(Fon_Delete_Field, 4);

                    EditingTable.Children.Add(TBox_Delete_Field);
                    Grid.SetColumn(TBox_Delete_Field, 0);
                    Grid.SetRow(TBox_Delete_Field, 8);

                    DeleteET.Items.Add(DeleteET1);
                    DeleteET.Items.Add(DeleteET2);
                    EditingTable.Children.Add(DeleteET);
                    Grid.SetColumn(DeleteET, 0);
                    Grid.SetRow(DeleteET, 10);

                    if (KeyDeleteET == 1) DeleteET1.IsSelected = true;
                    else if (KeyDeleteET == 2) DeleteET2.IsSelected = true;

                    EditingTable.Children.Add(Button_Delete_Field);
                    Grid.SetColumn(Button_Delete_Field, 0);
                    Grid.SetRow(Button_Delete_Field, 11);
                #endregion
                #region Элементы графика
                    for (int i = 0; i < 5; i++)
                    {
                        Rows[10, i] = new RowDefinition();
                        BlockForChart.RowDefinitions.Add(Rows[10, i]);
                    }

                    Rows[10, 0].Height = new GridLength(35);
                    Rows[10, 1].Height = new GridLength(100);
                    Rows[10, 2].Height = new GridLength(35);
                    Rows[10, 3].Height = new GridLength(35);

                    for (int i = 0; i < 2; i++)
                    {
                        Column[5, i] = new ColumnDefinition();
                        BlockForChart.ColumnDefinitions.Add(Column[5, i]);
                    }

                    Column[5, 0].Width = new GridLength(30);
                    
                    // Фон
                    TranslucentBlock = new Label();
                    TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                    BlockForChart.Children.Add(TranslucentBlock);
                    Grid.SetColumn(TranslucentBlock, 0);
                    Grid.SetRow(TranslucentBlock, 0);
                    Grid.SetColumnSpan(TranslucentBlock, 2);
                    Grid.SetRowSpan(TranslucentBlock, 5);

                    MinValueSlider = new Label();
                    MinValueSlider.Style = (Style)this.Resources["Label_Slider"];
                    MinValueSlider.Content = 0.5;
                    MinValueSlider.ToolTip = "Минимальное значение масштабирования";
                    BlockForChart.Children.Add(MinValueSlider);
                    Grid.SetColumn(MinValueSlider, 0);
                    Grid.SetRow(MinValueSlider, 2);

                    MaxValueSlider = new Label();
                    MaxValueSlider.Style = (Style)this.Resources["Label_Slider"];
                    MaxValueSlider.Content = 4;
                    MaxValueSlider.ToolTip = "Максимальное значение масштабирования";
                    BlockForChart.Children.Add(MaxValueSlider);
                    Grid.SetColumn(MaxValueSlider, 0);
                    Grid.SetRow(MaxValueSlider, 0);

                    _slider = new Slider();
                    _slider.Style = (Style)this.Resources["StyleSlider"];
                    _slider.ToolTip = "Масштабирование";
                    _slider.Value = 1;
                    _slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(ValueChanged);
                    BlockForChart.Children.Add(_slider);
                    Grid.SetColumn(_slider, 0);
                    Grid.SetRow(_slider, 1);

                    ValueSlider = new TextBox();
                    ValueSlider.Text = "1";
                    ValueSlider.ToolTip = "Текущее значение масштабирование";
                    ValueSlider.Style = (Style)this.Resources["TextBox_Slider"];
                    BlockForChart.Children.Add(ValueSlider);
                    Grid.SetColumn(ValueSlider, 0);
                    Grid.SetRow(ValueSlider, 3);

                    ScrollChart = new ScrollViewer();
                    ScrollChart.Style = (Style)this.Resources["ScrollViewerChart"];
                    BlockForChart.Children.Add(ScrollChart);
                    Grid.SetColumn(ScrollChart, 1);
                    Grid.SetRow(ScrollChart, 0);
                    Grid.SetRowSpan(ScrollChart, 5);

                    _Chart = new Canvas();
                #endregion
                #region Элементы отчёта
                    for (int i = 0; i < 4; i++)
                    {
                        Rows[3, i] = new RowDefinition();
                        BlockForResult.RowDefinitions.Add(Rows[3, i]);
                    }
                    
                    Rows[3, 0].Height = new GridLength(30);
                    Rows[3, 1].Height = new GridLength(35);
                    Rows[3, 2].Height = new GridLength(10);
                    BlockForResult.UpdateLayout();

                    // Фон
                    TranslucentBlock = new Label();
                    TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                    BlockForResult.Children.Add(TranslucentBlock);
                    Grid.SetColumn(TranslucentBlock, 0);
                    Grid.SetRow(TranslucentBlock, 0);
                    Grid.SetRowSpan(TranslucentBlock, 4);

                    LabelHeaderReport = new Label();
                    LabelHeaderReport.Style = (Style)this.Resources["StyleHeader"];

                    TextReport = new TextBox();
                    TextReport.Style = (Style)this.Resources["StyleText"];

                    BlockForResult.Children.Add(LabelHeaderReport);
                    Grid.SetColumn(LabelHeaderReport, 0);
                    Grid.SetRow(LabelHeaderReport, 1);

                    BlockForResult.Children.Add(TextReport);
                    Grid.SetColumn(TextReport, 0);
                    Grid.SetRow(TextReport, 3);
                #endregion
                #region Элементы пространства для сохранения результатов
                    
                    ScrollSaveResult = new ScrollViewer();
                    ScrollSaveResult.Style = (Style)this.Resources["ScrollViewer"];

                    ScrollSaveResult.Content = BlockForSaveResult;

                    for (int i = 0; i < 6; i++)
                    {
                        Rows[6, i] = new RowDefinition();
                        BlockForSaveResult.RowDefinitions.Add(Rows[6, i]);
                    }

                    Rows[6, 0].Height = new GridLength(20);
                    Rows[6, 1].Height = new GridLength(35);
                    Rows[6, 2].Height = new GridLength(20);
                    Rows[6, 3].Height = new GridLength(35);
                    Rows[6, 4].Height = new GridLength(30);

                    for (int i = 0; i < 3; i++)
                    {
                        Column[3, i] = new ColumnDefinition();
                        BlockForSaveResult.ColumnDefinitions.Add(Column[3, i]);
                    }

                    Column[3, 0].Width = new GridLength(140);
                    Column[3, 1].Width = new GridLength(200);

                    BlockForSaveResult.UpdateLayout();

                    LabelHeaderSaveResult = new Label();
                    LabelHeaderSaveResult.Style = (Style)this.Resources["StyleHeader"];
                    LabelHeaderSaveResult.Content = "Сохранение результатов";
                    BlockForSaveResult.Children.Add(LabelHeaderSaveResult);
                    Grid.SetRow(LabelHeaderSaveResult, 1);
                    Grid.SetColumn(LabelHeaderSaveResult, 0);
                    Grid.SetColumnSpan(LabelHeaderSaveResult, 3);

                    TextBox_FilesName = new TextBox[7];
                    Label_Elements_List = new Label[3];

                    Label_Elements_List[0] = new Label();
                    Label_Elements_List[0].Style = (Style)this.Resources["StyleForSaveResult"];
                    Label_Elements_List[0].Content = ElementsList[0];
                    BlockForSaveResult.Children.Add(Label_Elements_List[0]);
                    Grid.SetRow(Label_Elements_List[0], 3);
                    Grid.SetColumn(Label_Elements_List[0], 0);

                    TextBox_FilesName[0] = new TextBox();
                    TextBox_FilesName[0].Name = "TextBoxName0";
                    TextBox_FilesName[0].Style = (Style)this.Resources["TextBox_SaveResult"];
                    TextBox_FilesName[0].GotFocus += new RoutedEventHandler(GotFocusFN);
                    TextBox_FilesName[0].LostFocus += new RoutedEventHandler(LostFocusFN);
                    TextBox_FilesName[0].Text = default_files_name[0];
                    TextBox_FilesName[0].Margin = new Thickness(0, 5, 0, 5);
                    BlockForSaveResult.Children.Add(TextBox_FilesName[0]);
                    Grid.SetRow(TextBox_FilesName[0], 3);
                    Grid.SetColumn(TextBox_FilesName[0], 1);

                    Button_FolderSelection = new Button();
                    Button_FolderSelection.Name = "FolderSelection";
                    Button_FolderSelection.Content = "Выбрать";
                    Button_FolderSelection.Style = (Style)this.Resources["Button_Editing_Table"];
                    Button_FolderSelection.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    Button_FolderSelection.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    Button_FolderSelection.Margin = new Thickness(45, 1, 0, 0);
                    Button_FolderSelection.Width = 150;
                    BlockForSaveResult.Children.Add(Button_FolderSelection);
                    Grid.SetRow(Button_FolderSelection, 3);
                    Grid.SetColumn(Button_FolderSelection, 2);
                    Button_FolderSelection.Click += new RoutedEventHandler(FolderBrowserDialog_Click);

                    BlockForElementsList = new Grid();
                    BlockForSaveResult.Children.Add(BlockForElementsList);
                    Grid.SetRow(BlockForElementsList, 5);
                    Grid.SetColumn(BlockForElementsList, 0);
                    Grid.SetColumnSpan(BlockForElementsList, 3);

                    for (int i = 0; i < 12; i++)
                    {
                        Rows[9, i] = new RowDefinition();
                        BlockForElementsList.RowDefinitions.Add(Rows[9, i]);
                    }

                    Rows[9, 0].Height = new GridLength(35);
                    Rows[9, 1].Height = new GridLength(20);
                    Rows[9, 2].Height = new GridLength(55);
                    Rows[9, 3].Height = new GridLength(45);
                    Rows[9, 4].Height = new GridLength(45);
                    Rows[9, 5].Height = new GridLength(50);
                    Rows[9, 6].Height = new GridLength(65);
                    Rows[9, 7].Height = new GridLength(45);
                    Rows[9, 8].Height = new GridLength(45);
                    Rows[9, 9].Height = new GridLength(30);
                    Rows[9, 10].Height = new GridLength(70);
                    Rows[9, 11].Height = new GridLength(40);

                    for (int i = 0; i < 3; i++)
                    {
                        Column[4, i] = new ColumnDefinition();
                        BlockForElementsList.ColumnDefinitions.Add(Column[4, i]);
                    }

                    Column[4, 1].Width = new GridLength(30);
                    Column[4, 2].Width = new GridLength(200);

                    BlockForElementsList.UpdateLayout();

                    Label_Elements_List[1] = new Label();
                    Label_Elements_List[1].Style = (Style)this.Resources["SaveResultHeader"];
                    Label_Elements_List[1].Content = "Сохраняемые элементы";
                    BlockForElementsList.Children.Add(Label_Elements_List[1]);
                    Grid.SetRow(Label_Elements_List[1], 0);
                    Grid.SetColumn(Label_Elements_List[1], 0);
                    Grid.SetColumnSpan(Label_Elements_List[1], 2);

                    Label_Elements_List[2] = new Label();
                    Label_Elements_List[2].Style = (Style)this.Resources["SaveResultHeader"];
                    Label_Elements_List[2].Content = "Имена файлов";
                    BlockForElementsList.Children.Add(Label_Elements_List[2]);
                    Grid.SetRow(Label_Elements_List[2], 0);
                    Grid.SetColumn(Label_Elements_List[2], 2);

                    CheckBox_Elements_List = new CheckBox[7];
                    TextBox_Elements_List = new TextBox[7];
                    
                    for (int i = 1; i < 8; i++)
                    {
                        TextBox_Elements_List[i - 1] = new TextBox();
                        TextBox_Elements_List[i - 1].Style = (Style)this.Resources["TextElementsSaveResult"];
                        TextBox_Elements_List[i - 1].Text = ElementsList[i];
                        BlockForElementsList.Children.Add(TextBox_Elements_List[i - 1]);
                        Grid.SetRow(TextBox_Elements_List[i - 1], i + 1);
                        Grid.SetColumn(TextBox_Elements_List[i - 1], 0);

                        CheckBox_Elements_List[i - 1] = new CheckBox();
                        CheckBox_Elements_List[i - 1].Style = (Style)this.Resources["CheckBox_SaveResult"];
                        CheckBox_Elements_List[i - 1].Name = "Check" + i;
                        CheckBox_Elements_List[i - 1].Click += new RoutedEventHandler(CheckElements_SaveResults);
                        BlockForElementsList.Children.Add(CheckBox_Elements_List[i - 1]);
                        Grid.SetRow(CheckBox_Elements_List[i - 1], i + 1);
                        Grid.SetColumn(CheckBox_Elements_List[i - 1], 1);

                        if (i < 7)
                        {
                            TextBox_FilesName[i] = new TextBox();
                            TextBox_FilesName[i].Name = "TextBoxName" + i;
                            TextBox_FilesName[i].Style = (Style)this.Resources["TextBox_SaveResult"];
                            TextBox_FilesName[i].GotFocus += new RoutedEventHandler(GotFocusFN);
                            TextBox_FilesName[i].LostFocus += new RoutedEventHandler(LostFocusFN);
                            TextBox_FilesName[i].Text = default_files_name[i];
                            BlockForElementsList.Children.Add(TextBox_FilesName[i]);
                            Grid.SetRow(TextBox_FilesName[i], i+2);
                            Grid.SetColumn(TextBox_FilesName[i], 2);
                        }
                    }

                    SaveResults = new Button();
                    SaveResults.Name = "SaveResults";
                    SaveResults.Content = "Сохранить";
                    SaveResults.Style = (Style)this.Resources["Button_Editing_Table"];
                    SaveResults.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    SaveResults.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    SaveResults.Margin = new Thickness(45, 10, 0, 0);
                    SaveResults.Width = 150;
                    BlockForElementsList.Children.Add(SaveResults);
                    Grid.SetRow(SaveResults, 10);
                    Grid.SetColumn(SaveResults, 0);
                    Grid.SetColumnSpan(SaveResults, 3);
                    SaveResults.Click += new RoutedEventHandler(SaveResult_Click);
                    
                #endregion
                #region Элементы Bottom Menu
                    BlockForButtons = new Grid();

                    Rows[4, 0] = new RowDefinition();
                    Rows[4, 0].Height = new GridLength(30);
                    Rows[4, 1] = new RowDefinition();

                    FonForTCMenu = new Label();
                    FonForTCMenu.Style = (Style)this.Resources["HeaderET"];

                    TC_BottomMenu = new TabControl();
                    TC_BottomMenu.SelectionChanged += new SelectionChangedEventHandler(TCBottomMenu);
                    TC_BottomMenu.Style = (Style)this.Resources["TC_Style"];

                    TI_DataTable = new TabItem();
                    TI_DataTable.Style = (Style)this.Resources["TI_Style"];
                    TI_DataTable.Header = " Редактирование и визуализация ";

                    TI_LoadingData = new TabItem();
                    TI_LoadingData.Style = (Style)this.Resources["TI_Style"];
                    TI_LoadingData.Header = " Загрузка ";

                    TI_SaveData = new TabItem();
                    TI_SaveData.Style = (Style)this.Resources["TI_Style"];
                    TI_SaveData.Header = " Сохранение ";

                    GridForButton = new Grid();
                    Rows[5, 0] = new RowDefinition();
                    Rows[5, 1] = new RowDefinition();
                    Rows[5, 1].Height = new GridLength(0);
                    Rows[5, 2] = new RowDefinition();
                    Rows[5, 2].Height = new GridLength(0);
                    Rows[5, 3] = new RowDefinition();
                    Rows[5, 3].Height = new GridLength(30);
                    Rows[5, 4] = new RowDefinition();
                    Rows[5, 4].Height = new GridLength(25);
                    Column[2, 0] = new ColumnDefinition();
                    Column[2, 1] = new ColumnDefinition();
                    Column[2, 2] = new ColumnDefinition();
                    Column[2, 3] = new ColumnDefinition();
                    Column[2, 4] = new ColumnDefinition();

                    LoadFile = new ComboBox();
                    LoadFile.Name = "LoadFile";
                    LoadFile.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged_ComboBox);
                    LoadFile.Style = (Style)this.Resources["ComboBoxStyle"];
                    LoadFileTxt = new ComboBoxItem();
                    LoadFileTxt.Content = "Загрузить из txt файла";
                    LoadFileXml = new ComboBoxItem();
                    LoadFileXml.Content = "Загрузить из xml файла";

                    if (KeyLoadFile == 1) LoadFileTxt.IsSelected = true;
                    else if (KeyLoadFile == 2) LoadFileXml.IsSelected = true;

                    Button_Load = new Button();
                    Button_Load.Name = "Button_Load";
                    Button_Load.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Load.Content = "Загрузить";
                    Button_Load.Click += new RoutedEventHandler(LoadingOutFile_Click);

                    SaveFile = new ComboBox();
                    SaveFile.Name = "SaveFile";
                    SaveFile.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged_ComboBox);
                    SaveFile.Style = (Style)this.Resources["ComboBoxStyle"];
                    SaveFileTxt = new ComboBoxItem();
                    SaveFileTxt.Content = "Сохранить в txt файл";
                    SaveFileXml = new ComboBoxItem();
                    SaveFileXml.Content = "Сохранить в xml файл";

                    if (KeySaveFile == 1) SaveFileTxt.IsSelected = true;
                    else if (KeySaveFile == 2) SaveFileXml.IsSelected = true;

                    Button_Save = new Button();
                    Button_Save.Name = "Button_Save";
                    Button_Save.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Save.Content = "Сохранить";
                    Button_Save.Click += new RoutedEventHandler(SaveInFile_Click);

                    Button_EditingTable = new Button();
                    Button_EditingTable.Style = (Style)this.Resources["ButtonStyle"];
                    Button_EditingTable.Click += new RoutedEventHandler(EditingDataTable_Click);
                    Button_EditingTable.Content = "Редактировать таблицу";

                    Button_Chart = new Button();
                    Button_Chart.Name = "Chart";
                    Button_Chart.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Chart.Click += new RoutedEventHandler(Chart_Or_Table_Click);
                    Button_Chart.Content = "График";

                    Button_SaveChart = new Button();
                    Button_SaveChart.Style = (Style)this.Resources["ButtonStyle"];
                    Button_SaveChart.Click += new RoutedEventHandler(SaveChart_Click);
                    Button_SaveChart.Content = "Сохранить график";
                    Button_SaveChart.IsEnabled = true;

                    Button_Table = new Button();
                    Button_Table.Name = "Table";
                    Button_Table.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Table.Click += new RoutedEventHandler(Chart_Or_Table_Click);
                    Button_Table.Content = "Таблица";

                    Button_Start = new Button();
                    Button_Start.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Start.Content = "Построить модель";
                    Button_Start.Click += new RoutedEventHandler(BuildModel_Click);

                    Button_ResultTable = new Button();
                    Button_ResultTable.Name = "ResultTable";
                    Button_ResultTable.Style = (Style)this.Resources["ButtonStyle"];
                    Button_ResultTable.Click += new RoutedEventHandler(ResultButtons_Click);
                    Button_ResultTable.Content = "Данные";

                    Button_ResultChart = new Button();
                    Button_ResultChart.Name = "ResultChart";
                    Button_ResultChart.Style = (Style)this.Resources["ButtonStyle"];
                    Button_ResultChart.Click += new RoutedEventHandler(ResultButtons_Click);
                    Button_ResultChart.Content = "График";

                    Button_Result = new Button();
                    Button_Result.Name = "Result";
                    Button_Result.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Result.Click += new RoutedEventHandler(ResultButtons_Click);
                    Button_Result.Content = "Отчёт";

                    SavingResults = new Button();
                    SavingResults.Name = "SavingResults";
                    SavingResults.Style = (Style)this.Resources["ButtonStyle"];
                    SavingResults.Click += new RoutedEventHandler(ResultButtons_Click);
                    SavingResults.Content = "Сохранить";

                    MainButton_Prediction = new Button();
                    MainButton_Prediction.Name = "Prediction";
                    MainButton_Prediction.Style = (Style)this.Resources["ButtonStyle"];
                    MainButton_Prediction.Content = "Прогнозирование";
                    MainButton_Prediction.Click += new RoutedEventHandler(ResultButtons_Click);
                    MainButton_Prediction.Margin = new Thickness(12, 0, 18, 0);

                    Button_Home = new Button();
                    Button_Home.Name = "Home";
                    Button_Home.Style = (Style)this.Resources["ButtonStyle"];
                    Button_Home.Content = "На главную";
                    Button_Home.Click += new RoutedEventHandler(ResultButtons_Click);
                    Button_Home.Margin = new Thickness(12, 0, 18, 0);

                    if (KeyBottomMenu == 1) TI_DataTable.IsSelected = true;
                    else if (KeyBottomMenu == 2) TI_LoadingData.IsSelected = true;
                    else if (KeyBottomMenu == 3) TI_SaveData.IsSelected = true;

                    BlockContent.Children.Add(BlockForButtons);
                    Grid.SetColumn(BlockForButtons, 0);
                    Grid.SetRow(BlockForButtons, 2);

                    GridForButton.RowDefinitions.Add(Rows[5, 0]);
                    GridForButton.RowDefinitions.Add(Rows[5, 1]);
                    GridForButton.RowDefinitions.Add(Rows[5, 2]);
                    GridForButton.RowDefinitions.Add(Rows[5, 3]);
                    GridForButton.RowDefinitions.Add(Rows[5, 4]);
                    GridForButton.ColumnDefinitions.Add(Column[2, 0]);
                    GridForButton.ColumnDefinitions.Add(Column[2, 1]);
                    GridForButton.ColumnDefinitions.Add(Column[2, 2]);
                    GridForButton.ColumnDefinitions.Add(Column[2, 3]);
                    GridForButton.ColumnDefinitions.Add(Column[2, 4]);

                    TC_BottomMenu.Items.Add(TI_DataTable);
                    TC_BottomMenu.Items.Add(TI_LoadingData);
                    TC_BottomMenu.Items.Add(TI_SaveData);

                    LoadFile.Items.Add(LoadFileTxt);
                    LoadFile.Items.Add(LoadFileXml);

                    SaveFile.Items.Add(SaveFileTxt);
                    SaveFile.Items.Add(SaveFileXml);
                #endregion
            #endregion
        }

        /// <summary>
        /// Шаблон №1, по которому строится контент приложения
        /// </summary>
        private void Template_1()
        {
            // Обновление данных, содержащихся в таблице
            UpdateData();

            if (ErrorUpdate == false)
            {
                // Инициализация элементов шаблона №1
                if (KeyStartedT1)
                {
                    Template1_Elements();
                }

                // Удаление содержимого ContentGrid
                ContentGrid.Children.Clear();
                ContentGrid.ColumnDefinitions.Clear();
                ContentGrid.RowDefinitions.Clear();

                // Задание новой разметки строк и столбцов для ContentGrid
                Column[0, 0] = new ColumnDefinition();
                Column[0, 1] = new ColumnDefinition();
                Column[0, 2] = new ColumnDefinition();
                Column[0, 0].Width = new GridLength(210);
                Column[0, 1].Width = new GridLength(4);
                ContentGrid.ColumnDefinitions.Add(Column[0, 0]);
                ContentGrid.ColumnDefinitions.Add(Column[0, 1]);
                ContentGrid.ColumnDefinitions.Add(Column[0, 2]);

                // Добавление элемента BlockMenu в ContentGrid
                ContentGrid.Children.Add(BlockMenu);
                Grid.SetColumn(BlockMenu, 0);
                Grid.SetRow(BlockMenu, 0);

                // Добавление элемента BlockContent в ContentGrid
                ContentGrid.Children.Add(BlockContent);
                Grid.SetColumn(BlockContent, 2);
                Grid.SetRow(BlockContent, 0);

                SaveReport.IsEnabled = false;

                if (TypeContent == "Table")
                {
                    EditingDataTable1.IsEnabled = true;
                    
                    SpaceForTable();
                    SpaceForBottomMenu1();
                }
                else if (TypeContent == "Chart")
                {
                    EditingDataTable1.IsEnabled = false;

                    CreateChart();
                    SpaceForBottomMenu2();
                }
                else if (TypeContent == "Result")
                {
                    SaveReport.IsEnabled = true;
                    EditingDataTable1.IsEnabled = false;
                    EditingDataTable1.IsChecked = false;
                    EditingTableON = false;
                    EditingTableOpen = false;
                    
                    SpaceForBottomMenu3();

                    if (TypeContentResult == "Result") Report();
                    else if (TypeContentResult == "ResultTable") SpaceForTable();
                    else if (TypeContentResult == "ResultChart") CreateChartWithModel();
                    else if (TypeContentResult == "SaveResult")
                    {
                        BlockForContent.Children.Clear();
                        
                        TranslucentBlock = new Label();
                        TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
                        
                        BlockForContent.Children.Add(TranslucentBlock);
                        BlockForContent.Children.Add(ScrollSaveResult);

                        ScrollSaveResult.ScrollToHome();
                    }
                    else if (TypeContentResult == "Prediction")
                    {
                        Prediction();
                    }
                }
            }
        }

        /// <summary>
        /// Пространство, в котором отображается таблица
        /// </summary>
        private void SpaceForTable()
        {
            BlockForContent.Children.Clear();
            BlockForContent.Children.Add(BlockForTable);

            if (EditingTableON)
            {
                Column[1, 1].Width = new GridLength(4);
                Column[1, 2].Width = new GridLength(180);
            }
            else
            {
                Column[1, 1].Width = new GridLength(0);
                Column[1, 2].Width = new GridLength(0);
            }
            BlockForTable.UpdateLayout();

            // Построение таблицы
            if (KeyStartedT1)
            {
                FillingTable();
                KeyStartedT1 = false;
            }
            
            // Редактирование таблицы
            if (EditingTableON) MenuForEditingTable();
        }

        /// <summary>
        /// Меню для редактирования таблицы
        /// </summary>
        private void MenuForEditingTable()
        {
            MainEditingTable.Children.Clear();

            // Фон для меню
            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            MainEditingTable.Children.Add(TranslucentBlock);
            Grid.SetColumn(TranslucentBlock, 0);
            Grid.SetRow(TranslucentBlock, 0);

            // Добавление элемента EditingTable в BlockForTable
            MainEditingTable.Children.Add(EditingTable);
            Grid.SetColumn(EditingTable, 0);
            Grid.SetRow(EditingTable, 0);

            Txt_AmountRows.Text = Convert.ToString(Row);
            Txt_AmountColumns.Text = Convert.ToString(Col);

            EditingTableOpen = true;
        }

        /// <summary>
        /// График исходных данных
        /// </summary>
        private void CreateChart()
        {
            BlockForContent.Children.Clear();
            BlockForContent.Children.Add(BlockForChart);

            EditingDataTable1.IsEnabled = false;

            ScrollChart.ScrollToEnd();

            DS.Array = Array;
            _Chart = DS.Get_Chart_DataSet(500, 400);
            ScrollChart.Content = _Chart;
            _slider.Value = 1;
        }

        /// <summary>
        /// График исходных данных и построенной модели
        /// </summary>
        private void CreateChartWithModel()
        {
            BlockForContent.Children.Clear();
            BlockForContent.Children.Add(BlockForChart);

            EditingDataTable1.IsEnabled = false;

            ScrollChart.ScrollToEnd();

            DS.Array = Array;
            if (Forecast == null) _Chart = DS.Get_Chart_Model(RM, 500, 400);
            else _Chart = DS.Get_Chart_Model_And_Forecast(RM, RE, Factors, 500, 400);
            ScrollChart.Content = _Chart;
            _slider.Value = 1;
        }

        /// <summary>
        /// Результат работы приложения в виде собранной статистики и анализа
        /// </summary>
        private void Report()
        {
            int _Length = _Report.Length;
            string Info = "";

            EditingDataTable1.IsEnabled = false;

            BlockForContent.Children.Clear();
            BlockForContent.Children.Add(BlockForResult);
            LabelHeaderReport.Content = "Результаты";

            for (int i = 0; i < _Length; i++)
            {
                Info = Info + _Report[i] + "\n\n";
            }

            TextReport.Text = Info;
            TextReport.ScrollToHome();

            if (FindingBestModel.IsChecked == false && RM.GetInfoModel["Успешность построения модели"] == "Модель была построена некорректно, так как темпы роста неравномерны") 
                MessageBox.Show(RM.GetInfoModel["Успешность построения модели"] + ".\nДанная модель не подходит для прогнозирования.","Информация",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        /// <summary>
        /// Элементы для прогнозирования на основе построенной модели
        /// </summary>
        private void Prediction_Elements()
        {
            BlockForInputFactors.Children.Clear();
            BlockForInputFactors.ColumnDefinitions.Clear();
            BlockForInputFactors.RowDefinitions.Clear();

            BlockForPrediction.Children.Clear();
            BlockForPrediction.ColumnDefinitions.Clear();
            BlockForPrediction.RowDefinitions.Clear();

            #region Элементы пространства для ввода данных

            Label_Factors = new TextBox[Col - 1];
            Label_AVF = new TextBox[Col - 1];
            TextBox_Prediction = new TextBox[Col - 1];

            for (int i = 0; i < 5; i++)
            {
                Rows[11, i] = new RowDefinition();
                BlockForInputFactors.RowDefinitions.Add(Rows[11, i]);
                
            }

            Rows[11, 0].Height = new GridLength(10);
            Rows[11, 1].Height = new GridLength(55);
            Rows[11, 2].Height = new GridLength(10);
            Rows[11, 3].Height = new GridLength(55);
            Rows[11, 4].Height = new GridLength(10);

            for (int i = 5; i < 4 + Col; i++)
            {
                Rows[11, i] = new RowDefinition();
                BlockForInputFactors.RowDefinitions.Add(Rows[11, i]);
                Rows[11, i].Height = new GridLength(45);
            }

            for (int i = 4 + Col; i < 6 + Col; i++)
            {
                Rows[11, i] = new RowDefinition();
                BlockForInputFactors.RowDefinitions.Add(Rows[11, i]);
            }

            Rows[11, 4 + Col].Height = new GridLength(100);

            for (int i = 0; i < 3; i++)
            {
                Column[6, i] = new ColumnDefinition();
                BlockForInputFactors.ColumnDefinitions.Add(Column[6, i]);
            }

            Column[6, 0].Width = new GridLength(110);
            Column[6, 1].Width = new GridLength(160);

            // Фон
            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockForInputFactors.Children.Add(TranslucentBlock);
            Grid.SetColumn(TranslucentBlock, 0);
            Grid.SetRow(TranslucentBlock, 0);
            Grid.SetRowSpan(TranslucentBlock, Col + 6);
            Grid.SetColumnSpan(TranslucentBlock, 3);

            HeaderPrediction = new Label();
            HeaderPrediction.Style = (Style)this.Resources["StyleHeaderPrediction"];
            HeaderPrediction.Content = "Прогнозирование";
            BlockForInputFactors.Children.Add(HeaderPrediction);
            Grid.SetRow(HeaderPrediction, 1);
            Grid.SetColumn(HeaderPrediction, 0);
            Grid.SetColumnSpan(HeaderPrediction, 3);

            SubHeaderPrediction = new Label();
            SubHeaderPrediction.Style = (Style)this.Resources["StyleSubHeaderPrediction"];
            SubHeaderPrediction.FontSize = 15;
            SubHeaderPrediction.Content = "Уравнение: " + RM.GetInfoModel["Модель"];
            BlockForInputFactors.Children.Add(SubHeaderPrediction);
            Grid.SetRow(SubHeaderPrediction, 3);
            Grid.SetColumn(SubHeaderPrediction, 0);
            Grid.SetColumnSpan(SubHeaderPrediction, 3);

            double Temp1 = 0, Temp2 = 0;
            for (int i = 5; i < Col+4; i++)
            {
                Label_Factors[i - 5] = new TextBox();
                Label_Factors[i - 5].Text = "Фактор X" + (i - 4) + ": ";
                Label_Factors[i - 5].Style = (Style)this.Resources["TextElementsPrediction"];
                Label_Factors[i - 5].Margin = new Thickness(25, 5, 5, 5); 
                BlockForInputFactors.Children.Add(Label_Factors[i - 5]);
                Grid.SetRow(Label_Factors[i - 5], i);
                Grid.SetColumn(Label_Factors[i - 5], 0);

                Label_AVF[i - 5] = new TextBox();
                Temp1 = Math.Round(((DataEstimation["Максимальное значение X" + (i - 4)] - DataEstimation["Минимальное значение X" + (i - 4)]) / 3), 0);
                Temp2 = DataEstimation["Минимальное значение X" + (i - 4)] - Temp1;
                Temp1 = DataEstimation["Максимальное значение X" + (i - 4)] + Temp1;
                Label_AVF[i - 5].Text = "Рекомендуемые значения фактора X" + (i - 4) + " находятся в интервале от " + Math.Round(Temp2, 2) + " до " + Math.Round(Temp1, 2) + ".";
                Label_AVF[i - 5].Style = (Style)this.Resources["TextElementsPrediction"];
                BlockForInputFactors.Children.Add(Label_AVF[i - 5]);
                Grid.SetRow(Label_AVF[i - 5], i);
                Grid.SetColumn(Label_AVF[i - 5], 2);

                TextBox_Prediction[i - 5] = new TextBox();
                TextBox_Prediction[i - 5].Style = (Style)this.Resources["TextBox_Prediction"];
                TextBox_Prediction[i - 5].Foreground = new SolidColorBrush(Colors.Black);
                TextBox_Prediction[i - 5].PreviewTextInput += new TextCompositionEventHandler(TextChanged_DoubleNumber);
                TextBox_Prediction[i - 5].PreviewKeyDown += new KeyEventHandler(TextBox_PreviewKeyDown);
                TextBox_Prediction[i - 5].GotFocus += new RoutedEventHandler(GotFocusTableCell);
                TextBox_Prediction[i - 5].LostFocus += new RoutedEventHandler(LostFocusTableCell);
                TextBox_Prediction[i - 5].Text = "0";
                TextBox_Prediction[i - 5].Foreground = new SolidColorBrush(Color.FromArgb(146, 38, 44, 56));
                BlockForInputFactors.Children.Add(TextBox_Prediction[i - 5]);
                Grid.SetRow(TextBox_Prediction[i - 5], i);
                Grid.SetColumn(TextBox_Prediction[i - 5], 1);
            }

            Button_Prediction = new Button();
            Button_Prediction.Name = "OutputResult";
            Button_Prediction.Style = (Style)this.Resources["Button_Editing_Table"];
            Button_Prediction.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Button_Prediction.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Button_Prediction.Margin = new Thickness(26,35, 0, 0);
            Button_Prediction.Click += new RoutedEventHandler(Prediction_Click);
            Button_Prediction.Content = "Прогнозирование";
            Button_Prediction.Width = 170;
            BlockForInputFactors.Children.Add(Button_Prediction);
            Grid.SetRow(Button_Prediction, Col+4);
            Grid.SetColumn(Button_Prediction, 0);
            Grid.SetColumnSpan(Button_Prediction, 3);

            #endregion
            #region Элементы пространства для вывода результатов

            for (int i = 0; i < 7; i++)
            {
                Rows[12, i] = new RowDefinition();
                BlockForPrediction.RowDefinitions.Add(Rows[12, i]);
            }

            Rows[12, 0].Height = new GridLength(10);
            Rows[12, 1].Height = new GridLength(55);
            Rows[12, 2].Height = new GridLength(10);
            Rows[12, 4].Height = new GridLength(10);
            Rows[12, 5].Height = new GridLength(45);
            Rows[12, 6].Height = new GridLength(30);

            for (int i = 0; i < 2; i++)
            {
                Column[7, i] = new ColumnDefinition();
                BlockForPrediction.ColumnDefinitions.Add(Column[7, i]);
            }

            Column[7, 0].Width = new GridLength(220);

            // Фон
            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockForPrediction.Children.Add(TranslucentBlock);
            Grid.SetColumn(TranslucentBlock, 0);
            Grid.SetRow(TranslucentBlock, 0);
            Grid.SetRowSpan(TranslucentBlock, 7);
            Grid.SetColumnSpan(TranslucentBlock, 2);

            HeaderResultPrediction = new Label();
            HeaderResultPrediction.Style = (Style)this.Resources["StyleHeaderPrediction"];
            HeaderResultPrediction.Content = "Результаты прогнозирования";
            BlockForPrediction.Children.Add(HeaderResultPrediction);
            Grid.SetRow(HeaderResultPrediction, 1);
            Grid.SetColumn(HeaderResultPrediction, 0);
            Grid.SetColumnSpan(HeaderResultPrediction, 2);

            TextPrediction = new TextBox();
            TextPrediction.Style = (Style)this.Resources["StyleText"];
            BlockForPrediction.Children.Add(TextPrediction);
            Grid.SetColumn(TextPrediction, 0);
            Grid.SetRow(TextPrediction, 3);
            Grid.SetColumnSpan(TextPrediction, 2);

            Button_Input_DS_Pred = new Button();
            Button_Input_DS_Pred.Name = "InputFactors";
            Button_Input_DS_Pred.Style = (Style)this.Resources["Button_Editing_Table"];
            Button_Input_DS_Pred.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Button_Input_DS_Pred.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Button_Input_DS_Pred.Margin = new Thickness(26,0, 0, 0);
            Button_Input_DS_Pred.Click += new RoutedEventHandler(Prediction_Click);
            Button_Input_DS_Pred.Content = "Изменить данные";
            Button_Input_DS_Pred.Width = 170;
            BlockForPrediction.Children.Add(Button_Input_DS_Pred);
            Grid.SetRow(Button_Input_DS_Pred, 5);
            Grid.SetColumn(Button_Input_DS_Pred, 0);

            Button_SavePrediction = new Button();
            Button_SavePrediction.Style = (Style)this.Resources["Button_Editing_Table"];
            Button_SavePrediction.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Button_SavePrediction.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Button_SavePrediction.Margin = new Thickness(26, 0, 0, 0);
            Button_SavePrediction.Click += new RoutedEventHandler(SaveForecast_Click);
            Button_SavePrediction.Content = "Сохранить прогноз";
            Button_SavePrediction.Width = 170;
            BlockForPrediction.Children.Add(Button_SavePrediction);
            Grid.SetRow(Button_SavePrediction, 5);
            Grid.SetColumn(Button_SavePrediction, 1);

            #endregion
        }

        /// <summary>
        /// Прогнозирование на основе построенной модели
        /// </summary>
        private void Prediction()
        {
            if (IndicatorPrediction == 2)
            {
                TypeContentPrediction = "InputFactors";
                IndicatorPrediction = 1;
                Prediction_Elements();
            }

            BlockForContent.Children.Clear();

            if (TypeContentPrediction == "InputFactors")
            {
                ScrollPrediction = new ScrollViewer();
                ScrollPrediction.Style = (Style)this.Resources["ScrollViewer"];
                ScrollPrediction.Content = BlockForInputFactors;
                BlockForContent.Children.Add(ScrollPrediction);
            }
            else if (TypeContentPrediction == "OutputResult")
            {
                TextBox_Elements_List[6].IsEnabled = true;
                CheckBox_Elements_List[6].IsEnabled = true;
                TextBox_FilesName[6].IsEnabled = true;
                CheckBox_Elements_List[6].IsChecked = true;

                ScrollPrediction = new ScrollViewer();
                ScrollPrediction.Style = (Style)this.Resources["ScrollViewer"];
                ScrollPrediction.Content = BlockForPrediction;
                BlockForContent.Children.Add(ScrollPrediction);
            }
        }

        /// <summary>
        /// Меню, отвечающее за функционал программы
        /// </summary>
        #region SpaceForBottomMenu

        /// <summary>
        /// Первый тип меню
        /// </summary>
        private void SpaceForBottomMenu1()
        {
            BlockForButtons.Children.Clear();
            BlockForButtons.ColumnDefinitions.Clear();
            BlockForButtons.RowDefinitions.Clear();

            Rows[4, 0].Height = new GridLength(30);
            BlockForButtons.RowDefinitions.Add(Rows[4, 0]);
            BlockForButtons.RowDefinitions.Add(Rows[4, 1]);

            // Фон
            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockForButtons.Children.Add(TranslucentBlock);
            Grid.SetRowSpan(TranslucentBlock, 2);

            BlockForButtons.Children.Add(FonForTCMenu);
            Grid.SetColumn(FonForTCMenu, 0);
            Grid.SetRow(FonForTCMenu, 0);

            BlockForButtons.Children.Add(TC_BottomMenu);
            Grid.SetRowSpan(TC_BottomMenu, 2);

            Rows[5, 1].Height = new GridLength(0);
            Rows[5, 2].Height = new GridLength(0);
            Rows[5, 3].Height = new GridLength(30);
            Rows[5, 4].Height = new GridLength(25);
            Column[2, 0].Width = new GridLength(10);
            Column[2, 1].Width = new GridLength(210);
            Column[2, 2].Width = new GridLength(135);
            Column[2, 4].Width = new GridLength(195);
            GridForButton.UpdateLayout();
            GridForButton.Children.Clear();

            if (KeyLoadFile == 1) LoadFileTxt.IsSelected = true;
            else if (KeyLoadFile == 2) LoadFileXml.IsSelected = true;

            if (KeySaveFile == 1) SaveFileTxt.IsSelected = true;
            else if (KeySaveFile == 2) SaveFileXml.IsSelected = true;

            if (KeyBottomMenu == 1) TI_DataTable.IsSelected = true;
            else if (KeyBottomMenu == 2) TI_LoadingData.IsSelected = true;
            else if (KeyBottomMenu == 3) TI_SaveData.IsSelected = true;
            BottomMenu();
        }

        /// <summary>
        /// Выбор контента меню для TabControl
        /// </summary>
        private void BottomMenu()
        {
            GridForButton.Children.Clear();

            Button_Start.Margin = new Thickness(15, 0, 15, 0);
            GridForButton.Children.Add(Button_Start);
            Grid.SetColumn(Button_Start, 4);
            Grid.SetRow(Button_Start, 3);
            switch (KeyBottomMenu)
            {
                case 1:
                    TI_DataTable.Content = GridForButton;
                    GridForButton.Children.Add(Button_EditingTable);
                    Grid.SetColumn(Button_EditingTable, 1);
                    Grid.SetRow(Button_EditingTable, 3);
                    GridForButton.Children.Add(Button_Chart);
                    Grid.SetColumn(Button_Chart, 2);
                    Grid.SetRow(Button_Chart, 3);
                    break;
                case 2:
                    TI_LoadingData.Content = GridForButton;
                    GridForButton.Children.Add(LoadFile);
                    Grid.SetColumn(LoadFile, 1);
                    Grid.SetRow(LoadFile, 3);
                    GridForButton.Children.Add(Button_Load);
                    Grid.SetColumn(Button_Load, 2);
                    Grid.SetRow(Button_Load, 3);
                    break;
                case 3:
                    TI_SaveData.Content = GridForButton;
                    GridForButton.Children.Add(SaveFile);
                    Grid.SetColumn(SaveFile, 1);
                    Grid.SetRow(SaveFile, 3);
                    GridForButton.Children.Add(Button_Save);
                    Grid.SetColumn(Button_Save, 2);
                    Grid.SetRow(Button_Save, 3);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Второй тип меню
        /// </summary>
        private void SpaceForBottomMenu2()
        {
            BlockForButtons.Children.Clear();
            BlockForButtons.ColumnDefinitions.Clear();
            BlockForButtons.RowDefinitions.Clear();

            TI_DataTable.Content = "";
            TI_LoadingData.Content = "";
            TI_SaveData.Content = "";

            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockForButtons.Children.Add(TranslucentBlock);
            BlockForButtons.Children.Add(GridForButton);
            Rows[5, 1].Height = new GridLength(30);
            Rows[5, 2].Height = new GridLength(10);
            Rows[5, 3].Height = new GridLength(30);
            Rows[5, 4].Height = new GridLength(28);
            Column[2, 0].Width = new GridLength(13);
            Column[2, 1].Width = new GridLength(210);
            Column[2, 2].Width = new GridLength(135);
            Column[2, 4].Width = new GridLength(195);
            GridForButton.UpdateLayout();
            GridForButton.Children.Clear();

            GridForButton.Children.Add(Button_SaveChart);
            Grid.SetColumn(Button_SaveChart, 1);
            Grid.SetRow(Button_SaveChart, 3);

            GridForButton.Children.Add(Button_Table);
            Grid.SetColumn(Button_Table, 2);
            Grid.SetRow(Button_Table, 3);

            Button_Start.Margin = new Thickness(12, 0, 18, 0);
            GridForButton.Children.Add(Button_Start);
            Grid.SetColumn(Button_Start, 4);
            Grid.SetRow(Button_Start, 3);
        }

        /// <summary>
        /// Третий тип меню
        /// </summary>
        private void SpaceForBottomMenu3()
        {
            BlockForButtons.Children.Clear();
            BlockForButtons.ColumnDefinitions.Clear();
            BlockForButtons.RowDefinitions.Clear();

            TI_DataTable.Content = "";
            TI_LoadingData.Content = "";
            TI_SaveData.Content = "";

            // Фон
            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockForButtons.Children.Add(TranslucentBlock);
            BlockForButtons.Children.Add(GridForButton);
            Rows[5, 1].Height = new GridLength(30);
            Rows[5, 2].Height = new GridLength(20);
            Rows[5, 3].Height = new GridLength(30);
            Rows[5, 4].Height = new GridLength(28);
            Column[2, 0].Width = new GridLength(13);
            Column[2, 1].Width = new GridLength(180);
            Column[2, 2].Width = new GridLength(180);
            Column[2, 4].Width = new GridLength(180);
            GridForButton.UpdateLayout();
            GridForButton.Children.Clear();

            GridForButton.Children.Add(Button_ResultTable);
            Grid.SetColumn(Button_ResultTable, 1);
            Grid.SetRow(Button_ResultTable, 1);

            GridForButton.Children.Add(Button_ResultChart);
            Grid.SetColumn(Button_ResultChart, 2);
            Grid.SetRow(Button_ResultChart, 1);

            GridForButton.Children.Add(Button_Result);
            Grid.SetColumn(Button_Result, 1);
            Grid.SetRow(Button_Result, 3);

            GridForButton.Children.Add(SavingResults);
            Grid.SetColumn(SavingResults, 2);
            Grid.SetRow(SavingResults, 3);

            GridForButton.Children.Add(Button_Home);
            Grid.SetColumn(Button_Home, 4);
            Grid.SetRow(Button_Home, 3);

            GridForButton.Children.Add(MainButton_Prediction);
            Grid.SetColumn(MainButton_Prediction, 4);
            Grid.SetRow(MainButton_Prediction, 1);
        }

        #endregion

#endregion

#region Шаблон №2

        /// <summary>
        /// Инициализация элементов шаблона №2
        /// </summary>
        private void Template2_Elements()
        {
            BlockContentInfo = new Grid();

            Rows[7, 0] = new RowDefinition();
            Rows[7, 0].Height = new GridLength(10);
            Rows[7, 1] = new RowDefinition();
            Rows[7, 1].Height = new GridLength(70);
            Rows[7, 2] = new RowDefinition();
            Rows[7, 2].Height = new GridLength(0);
            Rows[7, 3] = new RowDefinition();
            BlockContentInfo.RowDefinitions.Add(Rows[7, 0]);
            BlockContentInfo.RowDefinitions.Add(Rows[7, 1]);
            BlockContentInfo.RowDefinitions.Add(Rows[7, 2]);
            BlockContentInfo.RowDefinitions.Add(Rows[7, 3]);

            LabelHeader = new Label();
            LabelHeader.Style = (Style)this.Resources["StyleHeader"];

            Text = new TextBox();
            Text.Style = (Style)this.Resources["StyleText"];

            LabelSubHeader = new Label();
            LabelSubHeader.Style = (Style)this.Resources["StyleSubHeader"];

            BlockMenuInfo = new Grid();

            Rows[8, 4] = new RowDefinition();
            Rows[8, 4].Height = new GridLength(25);
            Rows[8, 5] = new RowDefinition();
            Rows[8, 5].Height = new GridLength(45);
            Rows[8, 6] = new RowDefinition();
            Rows[8, 6].Height = new GridLength(45);
            Rows[8, 7] = new RowDefinition();
            BlockMenuInfo.RowDefinitions.Add(Rows[8, 4]);
            BlockMenuInfo.RowDefinitions.Add(Rows[8, 5]);
            BlockMenuInfo.RowDefinitions.Add(Rows[8, 6]);
            BlockMenuInfo.RowDefinitions.Add(Rows[8, 7]);

            BackToProgram = new Button();
            BackToProgram.Style = (Style)this.Resources["ButtonStyle"];
            BackToProgram.Content = "К программе";
            BackToProgram.Click += new RoutedEventHandler(BackToProgram_Click);

            HelpButton = new Button();
            HelpButton.Style = (Style)this.Resources["ButtonStyle"];
            HelpButton.Content = "Справка";
            HelpButton.Click += new RoutedEventHandler(Help_Click);
        }

        /// <summary>
        /// Шаблон №2, по которому строится контент приложения
        /// </summary>
        private void Template_2()
        {
            // Инициализация элементов шаблона №2
            if (KeyStartedT2)
            {
                Template2_Elements();
                KeyStartedT2 = false;
            }

            EditingDataTable1.IsEnabled = false;

            ContentGrid.Children.Clear();
            ContentGrid.ColumnDefinitions.Clear();
            ContentGrid.RowDefinitions.Clear();

            // Задание новой разметки строк и столбцов для ContentGrid
            Column[0, 0] = new ColumnDefinition();
            Column[0, 1] = new ColumnDefinition();
            Column[0, 2] = new ColumnDefinition();
            Column[0, 1].Width = new GridLength(4);
            Column[0, 2].Width = new GridLength(200);
            ContentGrid.ColumnDefinitions.Add(Column[0, 0]);
            ContentGrid.ColumnDefinitions.Add(Column[0, 1]);
            ContentGrid.ColumnDefinitions.Add(Column[0, 2]);

            ContentGrid.Children.Add(BlockContentInfo);
            Grid.SetColumn(BlockContentInfo, 0);
            Grid.SetRow(BlockContentInfo, 0);

            ContentGrid.Children.Add(BlockMenuInfo);
            Grid.SetColumn(BlockMenuInfo, 2);
            Grid.SetRow(BlockMenuInfo, 0);

            BlockContentInfo.Children.Clear();

            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockContentInfo.Children.Add(TranslucentBlock);
            Grid.SetRowSpan(TranslucentBlock, 4);

            BlockContentInfo.Children.Add(LabelHeader);
            Grid.SetColumn(LabelHeader, 0);
            Grid.SetRow(LabelHeader, 1);

            BlockContentInfo.Children.Add(Text);
            Grid.SetColumn(Text, 0);
            Grid.SetRow(Text, 3);

            BlockMenuInfo.Children.Clear();

            TranslucentBlock = new Label();
            TranslucentBlock.Style = (Style)this.Resources["TranslucentBlock"];
            BlockMenuInfo.Children.Add(TranslucentBlock);
            Grid.SetRowSpan(TranslucentBlock, 4);

            BlockMenuInfo.Children.Add(BackToProgram);
            Grid.SetColumn(BackToProgram, 0);
            Grid.SetRow(BackToProgram, 1);
        }

        /// <summary>
        /// Информация об авторе
        /// </summary>
        /// <param name="sender">Кнопка, которая была нажата</param>
        /// <param name="e">Событие</param>
        private void AboutAuthor_Click(object sender, RoutedEventArgs e)
        {
            Info = "";
            Template_2();

            Rows[7, 2].Height = new GridLength(0);
            BlockContentInfo.UpdateLayout();

            BlockMenuInfo.Children.Add(HelpButton);
            Grid.SetColumn(HelpButton, 0);
            Grid.SetRow(HelpButton, 2);

            LabelHeader.Content = "О разработчике";
            Info = "Данное приложение разработал: \n\n" +
                   "Студент 4 курса ТГПУ имени Льва Николаевича Толстого, группы 121131, " +
                   "факультета математики, физики и информатики, Большаков Антон Александрович. \n\n" +
                   "Направление / Специальность: Фундаментальная информатика и информационные технологии \n\n" +
                   "Профиль / Специализация: Открытые информационные системы.";
            Text.Text = Info;
        }

        /// <summary>
        /// Информация о приложении
        /// </summary>
        /// <param name="sender">Кнопка, которая была нажата</param>
        /// <param name="e">Событие</param>
        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            Info = "";
            string _path = Environment.CurrentDirectory;
            _path = _path + "/Options";

            if (LoadingInfo(_path, "About_Program"))
            {
                Template_2();

                Rows[7, 2].Height = new GridLength(0);
                BlockContentInfo.UpdateLayout();

                BlockMenuInfo.Children.Add(HelpButton);
                Grid.SetColumn(HelpButton, 0);
                Grid.SetRow(HelpButton, 2);

                LabelHeader.Content = "О программе";

                Text.Text = Info;
            }
        }

        /// <summary>
        /// Справка
        /// </summary>
        /// <param name="sender">Кнопка, которая была нажата</param>
        /// <param name="e">Событие</param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Info = "";
            string _path = Environment.CurrentDirectory;
            _path = _path + "/Options";
            
            if (LoadingInfo(_path, "Reference"))
            {
                Template_2();

                Rows[7, 2].Height = new GridLength(0);
                BlockContentInfo.UpdateLayout();

                LabelHeader.Content = "Справка";

                Text.Text = Info;
            }
        }

#endregion



#region Загрузка и сохранение данных

        /// <summary>
        /// Загрузка исходных данных в формате .txt или .xml
        /// </summary>
        /// <param name="sender">Кнопка или элемент меню</param>
        /// <param name="e">Событие</param>
        private void LoadingOutFile_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();

            if (ErrorUpdate == false)
            {
                string ChooseName = "";
                try
                {
                    Button ChooseButton = (Button)sender;
                    ChooseName = ChooseButton.Name;
                }
                catch
                {
                    try
                    {
                        MenuItem ChooseMenuItem = (MenuItem)sender;
                        ChooseName = ChooseMenuItem.Name;
                    }
                    catch
                    {
                        ChooseName = "";
                    }
                }

                if (ChooseName == "Button_Load")
                {
                    if (LoadFileTxt.IsSelected) KeyLoadFile = 1;
                    else if (LoadFileXml.IsSelected) KeyLoadFile = 2;
                }
                else if (ChooseName == "LoadTxt") KeyLoadFile = 1;
                else if (ChooseName == "LoadXml") KeyLoadFile = 2;

                string PathToFile = GetPathOpenFile();

                if (KeyLoadFile == 1) DS.OutFile(PathToFile);
                else if (KeyLoadFile == 2) DS.OutXML(PathToFile);

                _Error = DS.GetError;
                ErrorString = DS.GetErrorString;

                if (_Error != null)
                {
                    if (ErrorString != "Файл не выбран") MessageBox.Show("Не удалось загрузить данные из файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Row = DS.GetRows;
                    Col = DS.GetCols;
                    Array = new double[Row, Col];
                    Array = DS.Array;

                    // Преобразования

                    for (int i = 0; i < Row; i++)
                    {
                        for (int j = 0; j < Col; j++)
                        {
                            Array[i, j] = Array[i, j] * CoefX;
                        }
                    }

                    DS.Array = Array;
                    Array = DS.Array;

                    // Преобразования

                    FillingTable();
                }
            }
        }

        /// <summary>
        /// Сохранение исходных данных в формате .txt или .xml
        /// </summary>
        /// <param name="sender">Кнопка или элемент меню</param>
        /// <param name="e">Событие</param>
        private void SaveInFile_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();

            if (ErrorUpdate == false)
            {
                string ChooseName = "";
                try
                {
                    Button ChooseButton = (Button)sender;
                    ChooseName = ChooseButton.Name;
                }
                catch
                {
                    try
                    {
                        MenuItem ChooseMenuItem = (MenuItem)sender;
                        ChooseName = ChooseMenuItem.Name;
                    }
                    catch
                    {
                        ChooseName = "";
                    }
                }

                if (ChooseName == "Button_Save")
                {
                    if (SaveFileTxt.IsSelected) KeySaveFile = 1;
                    else if (SaveFileXml.IsSelected) KeySaveFile = 2;
                }
                else if (ChooseName == "SaveTxt") KeySaveFile = 1;
                else if (ChooseName == "SaveXml") KeySaveFile = 2;

                string PathToFile = GetPathSaveFile();
                DS.Array = Array;

                if (KeySaveFile == 1) DS.InFile(PathToFile);
                else if (KeySaveFile == 2) DS.InXML(PathToFile);

                _Error = DS.GetError;
                ErrorString = DS.GetErrorString;

                if (_Error != null)
                {
                    if (ErrorString != "Файл не выбран")
                    {
                        if (ErrorString == "Некорректные данные") MessageBox.Show("Набор данных некорректен, пожалуйста, проверьте сохраняемые данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else MessageBox.Show("Не удалось сохранить данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (KeySaveFile == 1) MessageBox.Show("Данные были успешно сохранены в txt файл!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    else if (KeySaveFile == 2) MessageBox.Show("Данные были успешно сохранены в xml файл!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Сохранение графика, как изображения в формате .png
        /// </summary>
        /// <param name="sender">Кнопка или элемент меню</param>
        /// <param name="e">Событие</param>
        private void SaveChart_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();

            if (ErrorUpdate == false)
            {
                string PathToFile = GetPathSaveChart();
                DS.Array = Array;
                if (TypeContent == "Result") _Chart = DS.Get_Chart_Model(RM);
                else _Chart = DS.Get_Chart_DataSet();

                DS.Save_Chart(_Chart, PathToFile);

                _Error = DS.GetError;
                ErrorString = DS.GetErrorString;

                if (_Error != null)
                {
                    if (ErrorString != "Файл не выбран")
                    {
                        if (ErrorString == "Некорректные данные") MessageBox.Show("Набор данных некорректен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else MessageBox.Show("Не удалось сохранить график!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("График был успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Сохранение прогноза в файл с расширением .txt
        /// </summary>
        /// <param name="sender">Кнопка, которая была нажата</param>
        /// <param name="e">Событие</param>
        private void SaveForecast_Click(object sender, RoutedEventArgs e)
        {
            string PathToFile = GetPathSaveFile();

            try
            {
                if (!string.IsNullOrEmpty(PathToFile))
                {
                    PathToFile = System.IO.Path.ChangeExtension(PathToFile, "txt");
                    StreamWriter File = new StreamWriter(PathToFile);

                    for (int i = 0; i < TextForecast.Length; i++)
                    {
                        File.WriteLine(TextForecast[i]);
                    }
                    File.Close();

                    MessageBox.Show("Данные были успешно сохранены в txt файл!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка информации для приложения
        /// </summary>
        /// <param name="PathToFile">Путь к файлу с данными</param>
        /// <param name="FileName">Название файла</param>
        private bool LoadingInfo(string PathToFile, string FileName)
        {
            string _File = PathToFile + "/" + FileName;

            try
            {
                FileInfo _FileInfo = new FileInfo(_File);

                string File_CreationTime, File_LastWriteTime;

                File_CreationTime = Convert.ToString(_FileInfo.CreationTime);
                File_LastWriteTime = Convert.ToString(_FileInfo.LastWriteTime);

                if (!string.IsNullOrEmpty(_File))
                {
                    StreamReader File = new StreamReader(_File);
                    string Line;
                    Info = "";
                    while (File.EndOfStream != true)
                    {
                        Line = File.ReadLine();
                        Info = Info + Line + "\n";
                    }
                    File.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Данные в файле были повреждены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Template_1();
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Файл был повреждён!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Template_1();
                return false;
            }
        }

        /// <summary>
        /// Выбор файла, из которого нужно загрузить исходные данные
        /// </summary>
        /// <returns>Путь к файлу</returns>
        private string GetPathOpenFile()
        {
            Microsoft.Win32.OpenFileDialog Choose_File = new Microsoft.Win32.OpenFileDialog();
            if (KeyLoadFile == 1) Choose_File.Filter = "Txt files (*.txt)|*.txt";
            else if (KeyLoadFile == 2) Choose_File.Filter = "Xml files (*.xml)|*.xml";

            if (Choose_File.ShowDialog() == true) return Choose_File.FileName;
            else return null;
        }

        /// <summary>
        /// Выбор файла, в который нужно сохранить исходные данные
        /// </summary>
        /// <returns>Путь к файлу</returns>
        private string GetPathSaveFile()
        {
            Microsoft.Win32.SaveFileDialog Choose_File = new Microsoft.Win32.SaveFileDialog();
            if (KeySaveFile == 1) Choose_File.Filter = "Txt files (*.txt)|*.txt";
            else if (KeySaveFile == 2) Choose_File.Filter = "Xml files (*.xml)|*.xml";

            if (Choose_File.ShowDialog() == true) return Choose_File.FileName;
            else return null;
        }

        /// <summary>
        /// Выбор изображения для сохранения графика
        /// </summary>
        /// <returns>Путь к изображению</returns>
        private string GetPathSaveChart()
        {
            Microsoft.Win32.SaveFileDialog SaveChart = new Microsoft.Win32.SaveFileDialog();
            SaveChart.Filter = "Image (.PNG)|*.PNG";

            if (SaveChart.ShowDialog() == true) return SaveChart.FileName;
            else return null;
        }

#endregion

#region Работа с таблицей и данными

        /// <summary>
        /// Функция обновления данных таблицы
        /// </summary>
        private void UpdateData()
        {
            int i = 0, j = 0;
            try
            {
                Array = new double[Row, Col];
                string line = "";
                int index = 0;
                for (i = 1; i <= Row; i++)
                {
                    for (j = 1; j <= Col; j++)
                    {
                        if (DataTable_Cell != null && (i < DataTable_Cell.GetLength(0) && j < DataTable_Cell.GetLength(1)))
                        {
                            if (DataTable_Cell[i, j].Text == "") Array[i - 1, j - 1] = 0;
                            else
                            {
                                line = DataTable_Cell[i, j].Text;
                                index = DataTable_Cell[i, j].Text.Length;
                                if (line[0] == ',') DataTable_Cell[i, j].Text = "0" + DataTable_Cell[i, j].Text;
                                if (line[index - 1] == ',') DataTable_Cell[i, j].Text = DataTable_Cell[i, j].Text + "0";
                                if (Convert.ToDouble(DataTable_Cell[i, j].Text) > 100000000000000) Array[i - 1, j - 1] = 100000000000000;
                                else if (Convert.ToDouble(DataTable_Cell[i, j].Text) < -100000000000000) Array[i - 1, j - 1] = -100000000000000;
                                else Array[i - 1, j - 1] = Convert.ToDouble(DataTable_Cell[i, j].Text);
                            }
                            DataTable_Cell[i, j].Text = Convert.ToString(Array[i - 1, j - 1]);
                        }
                    }
                }
                ErrorUpdate = false;
            }
            catch
            {
                MessageBox.Show("Некорректные данные в ячейке: (Строка " + i + " : Столбец " + j + ") !", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorUpdate = true;
            }
        }

        /// <summary>
        /// Функция обновления значений факторов для прогнозирования
        /// </summary>
        private void UpdateValueFactors()
        {
            int i = 0;
            try
            {
                Factors = new double[Col-1];
                string line = "";
                int index = 0;
                for (i = 0; i < Col-1; i++)
                {
                    if (TextBox_Prediction != null && i < TextBox_Prediction.Length)
                    {
                        if (TextBox_Prediction[i].Text == "") Factors[i] = 0;
                        else
                        {
                            line = TextBox_Prediction[i].Text;
                            index = TextBox_Prediction[i].Text.Length;
                            if (line[0] == ',') TextBox_Prediction[i].Text = "0" + TextBox_Prediction[i].Text;
                            if (line[index - 1] == ',') TextBox_Prediction[i].Text = TextBox_Prediction[i].Text + "0";
                            if (Convert.ToDouble(TextBox_Prediction[i].Text) > 100000000000000) Factors[i] = 100000000000000;
                            else if (Convert.ToDouble(TextBox_Prediction[i].Text) < -100000000000000) Factors[i] = -100000000000000;
                            else Factors[i] = Convert.ToDouble(TextBox_Prediction[i].Text);
                        }
                        TextBox_Prediction[i].Text = Convert.ToString(Factors[i]);
                    }
                }
                ErrorUpdateFactors = false;
            }
            catch
            {
                MessageBox.Show("Фактор X" + (i + 1) + "имеет некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorUpdateFactors = true;
            }
        }

        /// <summary>
        /// Функция построения таблицы
        /// </summary>
        private void FillingTable()
        {
            if (EditingTableOpen)
            {
                Txt_AmountRows.Text = Convert.ToString(Row);
                Txt_AmountColumns.Text = Convert.ToString(Col);
            }
            DataTable.Items.Clear();

            Table_Row = new Grid[Row + 1];
            Table_Column = new ColumnDefinition[Col + 1];
            DataTable_Cell = new TextBox[Row + 1, Col + 1];

            Table_Row[0] = new Grid();
            Table_Row[0].Height = 25;
            DataTable.Items.Add(Table_Row[0]);

            for (int j = 0; j <= Col; j++)
            {
                Table_Column[j] = new ColumnDefinition();
                DataTable_Cell[0, j] = new TextBox();

                if (j == 0)
                {
                    Table_Column[j].Width = new GridLength(35);
                    Table_Column[j].MinWidth = 35;
                    DataTable_Cell[0, j].Text = "№";
                }
                else if (j == 1) DataTable_Cell[0, j].Text = "Y";
                else DataTable_Cell[0, j].Text = "X" + (j - 1);

                Table_Row[0].ColumnDefinitions.Add(Table_Column[j]);
                DataTable_Cell[0, j].Style = (Style)this.Resources["Header_CellTable"];
                Table_Row[0].Children.Add(DataTable_Cell[0, j]);
                Grid.SetColumn(DataTable_Cell[0, j], j);
                Grid.SetRow(DataTable_Cell[0, j], 0);
            }

            for (int i = 1; i <= Row; i++)
            {
                Table_Row[i] = new Grid();
                Table_Row[i].Height = 25;
                DataTable.Items.Add(Table_Row[i]);

                Table_Column[0] = new ColumnDefinition();
                Table_Column[0].Width = new GridLength(35);
                Table_Column[0].MinWidth = 35;

                Table_Row[i].ColumnDefinitions.Add(Table_Column[0]);
                DataTable_Cell[i, 0] = new TextBox();
                DataTable_Cell[i, 0].Text = Convert.ToString(i);
                DataTable_Cell[i, 0].Style = (Style)this.Resources["Header_CellTable"];
                Table_Row[i].Children.Add(DataTable_Cell[i, 0]);
                Grid.SetColumn(DataTable_Cell[i, 0], 0);
                Grid.SetRow(DataTable_Cell[i, 0], i);

                for (int j = 1; j <= Col; j++)
                {
                    Table_Column[j] = new ColumnDefinition();

                    DataTable_Cell[i, j] = new TextBox();
                    DataTable_Cell[i, j].PreviewTextInput += new TextCompositionEventHandler(TextChanged_DoubleNumber);
                    DataTable_Cell[i, j].PreviewKeyDown += new KeyEventHandler(TextBox_PreviewKeyDown);
                    DataTable_Cell[i, j].CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnPasteOrCutCommand));
                    DataTable_Cell[i, j].CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnPasteOrCutCommand));
                    DataTable_Cell[i, j].Style = (Style)this.Resources["CellTable"];
                    DataTable_Cell[i, j].MaxLength = 17;
                    DataTable_Cell[i, j].IsReadOnly = false;
                    DataTable_Cell[i, j].GotFocus += new RoutedEventHandler(GotFocusTableCell);
                    DataTable_Cell[i, j].LostFocus += new RoutedEventHandler(LostFocusTableCell);
                    DataTable_Cell[i, j].Foreground = new SolidColorBrush(Color.FromArgb(146, 38, 44, 56));
                    if ((i <= Array.GetLength(0)) && (j <= Array.GetLength(1)))
                    {
                        DataTable_Cell[i, j].Text = Convert.ToString(Array[i - 1, j - 1]);
                        if (Array[i - 1, j - 1] != 0) DataTable_Cell[i, j].Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else DataTable_Cell[i, j].Text = "0";

                    Table_Row[i].ColumnDefinitions.Add(Table_Column[j]);
                    Table_Row[i].Children.Add(DataTable_Cell[i, j]);
                    Grid.SetColumn(DataTable_Cell[i, j], j);
                    Grid.SetRow(DataTable_Cell[i, j], i);
                }
            }
        }

#endregion


            
#region События, отвечающие за нажатие
        
        /// <summary>
        /// Загрузка шаблона №1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToProgram_Click(object sender, RoutedEventArgs e)
        {
            Template_1();
        }

        /// <summary>
        /// Построение модели регрессии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildModel_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();

            if (ErrorUpdate == false)
            {
                DS.Array = Array;

                Forecast = null;

                if (FindingBestModel.IsChecked == false)
                {
                    if (LinearModel.IsChecked == true) RM.BuildLinearModel(DS);
                    else if (SemilogModel.IsChecked == true) RM.BuildSemilogModel(DS);
                    else if (ParabolicModel.IsChecked == true) RM.BuildParabolicModel(DS);
                    else if (DegreesModel.IsChecked == true) RM.BuildDegreesModel(DS);
                    else if (AdaptiveModel.IsChecked == true) RM.BuildAdaptiveModel(DS);
                    else if (BrownModel.IsChecked == true) RM.BuildBrownModel_LinaerTrend(DS, Smoothing_parameter);

                    /* Замер времени построения всех моделей
                    
                    string Rep = "";
                    
                    Stopwatch _Time = new Stopwatch();
                    _Time.Start();
                    RM.BuildAdaptiveModel(DS);
                    _Time.Stop();
                    Rep = Rep + "" + _Time.Elapsed;

                    _Time.Start();
                    RM.BuildLinearModel(DS);
                    _Time.Stop();
                    Rep = Rep + " " + _Time.Elapsed;

                    _Time.Start();
                    RM.BuildSemilogModel(DS);
                    _Time.Stop();
                    Rep = Rep + " " + _Time.Elapsed;

                    _Time.Start();
                    RM.BuildParabolicModel(DS);
                    _Time.Stop();
                    Rep = Rep + " " + _Time.Elapsed;

                    _Time.Start();
                    RM.BuildDegreesModel(DS);
                    _Time.Stop();
                    Rep = Rep + " " + _Time.Elapsed;

                    _Time.Start();
                    RM.BuildBrownModel_LinaerTrend(DS, Smoothing_parameter);
                    _Time.Stop();
                    Rep = Rep + " " + _Time.Elapsed;

                    MessageBox.Show(Rep);
                    StreamWriter File1 = new StreamWriter("TestFile.txt");
                    File1.WriteLine(Rep);
                    File1.Close(); */

                    if (RM.GetError == null)
                    {
                        Det = RM.GetDeterminant;
                        if (Det == 0) MessageBox.Show("Невозможно найти решение. Для текущего набора данных определитель обратной матрицы равен 0.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                        {
                            TypeContent = "Result";
                            RE.Estimate(DS, RM);
                            DataEstimation = RE.GetDataEstimation;
                            InfoModel = RM.GetInfoModel;
                            _Report = RE.GetReport;
                            Coefficients = RM.GetCoefficients;
                        }
                    }
                    else MessageBox.Show("Набор данных некорректен, пожалуйста, проверьте данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    TypeContent = "Result";

                    RE.MostSuitableModel(DS, RM);
                    Coefficients = RM.GetCoefficients;
                    DataEstimation = RE.GetDataEstimation;
                    _Report = RE.GetReport;
                }
            }

            if ((RM.GetError == null && Det != 0) || FindingBestModel.IsChecked == true)
            {
                for (int i = 1; i <= Row; i++)
                {
                    for (int j = 1; j <= Col; j++)
                    {
                        DataTable_Cell[i, j].IsReadOnly = true;
                    }
                }

                IndicatorPrediction = 2;

                for (int i = 0; i < 6; i++)
                {
                    CheckBox_Elements_List[i].IsChecked = true;
                }

                TextBox_Elements_List[6].IsEnabled = false;
                CheckBox_Elements_List[6].IsEnabled = false;
                TextBox_FilesName[6].IsEnabled = false;
                CheckBox_Elements_List[6].IsChecked = false;

                if (Col != 2)
                {
                    TextBox_Elements_List[3].IsEnabled = false;
                    CheckBox_Elements_List[3].IsEnabled = false;
                    TextBox_FilesName[3].IsEnabled = false;
                    CheckBox_Elements_List[3].IsChecked = false;

                    TextBox_Elements_List[4].IsEnabled = false;
                    CheckBox_Elements_List[4].IsEnabled = false;
                    TextBox_FilesName[4].IsEnabled = false;
                    CheckBox_Elements_List[4].IsChecked = false;
                }
                else
                {
                    TextBox_Elements_List[3].IsEnabled = true;
                    CheckBox_Elements_List[3].IsEnabled = true;
                    TextBox_FilesName[3].IsEnabled = true;
                    CheckBox_Elements_List[3].IsChecked = true;

                    TextBox_Elements_List[4].IsEnabled = true;
                    CheckBox_Elements_List[4].IsEnabled = true;
                    TextBox_FilesName[4].IsEnabled = true;
                    CheckBox_Elements_List[4].IsChecked = true;
                }

                LinearModel.IsEnabled = false;
                SemilogModel.IsEnabled = false;
                ParabolicModel.IsEnabled = false;
                DegreesModel.IsEnabled = false;
                AdaptiveModel.IsEnabled = false;
                BrownModel.IsEnabled = false;
                FindingBestModel.IsEnabled = false;

                Template_1();
            }
        }

        /// <summary>
        /// Выбор отображаемого контента для результата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultButtons_Click(object sender, RoutedEventArgs e)
        {
            string ChooseName = "";
            try
            {
                Button ChooseButton = (Button)sender;
                ChooseName = ChooseButton.Name;
            }
            catch
            {
                try
                {
                    MenuItem ChooseMenuItem = (MenuItem)sender;
                    ChooseName = ChooseMenuItem.Name;
                }
                catch
                {
                    ChooseName = "";
                }
            }

            switch (ChooseName)
            {
                case "ResultTable":
                    TypeContent = "Result";
                    TypeContentResult = "ResultTable";
                    break;
                case "ResultChart":
                    TypeContent = "Result";
                    if (Col == 2 && Row > 0) TypeContentResult = "ResultChart";
                    else
                    {
                        if (Row <= 0) MessageBox.Show("Данные, на основе которых строится график, отсутствуют.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (Col != 2) MessageBox.Show("Данная программа строит только двумерные графики.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case "SavingResults":
                    TypeContent = "Result";
                    TypeContentResult = "SaveResult";
                    break;
                case "SaveReport":
                    TypeContent = "Result";
                    TypeContentResult = "SaveResult";
                    break;
                case "Result":
                    TypeContent = "Result";
                    TypeContentResult = "Result";
                    break;
                case "Prediction":
                    TypeContent = "Result";
                    TypeContentResult = "Prediction";
                    break;
                case "Home":
                    TypeContent = "Table";
                    TypeContentResult = "Result";

                    LinearModel.IsEnabled = true;
                    SemilogModel.IsEnabled = true;
                    ParabolicModel.IsEnabled = true;
                    DegreesModel.IsEnabled = true;
                    AdaptiveModel.IsEnabled = true;
                    BrownModel.IsEnabled = true;
                    FindingBestModel.IsEnabled = true;

                    for (int i = 1; i <= Row; i++)
                    {
                        for (int j = 1; j <= Col; j++)
                        {
                            DataTable_Cell[i, j].IsReadOnly = false;
                        }
                    }
                    break;
                default:
                    break;
            }
            Template_1();
        }

        /// <summary>
        /// Выбор размеров окна приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Screen_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Choose = (MenuItem)sender;
            switch (Choose.Name)
            {
                case "Screen1":
                    HeightScreen = 600;
                    WidthScreen = 785;
                    break;
                case "Screen2":
                    HeightScreen = 600;
                    WidthScreen = 850;
                    break;
                case "Screen3":
                    HeightScreen = 600;
                    WidthScreen = 930;
                    break;
                case "Screen4":
                    HeightScreen = 650;
                    WidthScreen = 1024;
                    break;
                case "Screen5":
                    HeightScreen = 710;
                    WidthScreen = 1160;
                    break;
                default:
                    break;
            }
            MenuItemScreen();
            key = true; RegressionModels.Height = HeightScreen; key = false;
            RegressionModels.Width = WidthScreen;           
        }

        /// <summary>
        /// Выбор отображения во всеь экран или в оконном режиме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeView_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Choose = (MenuItem)sender;
            switch (Choose.Name)
            {
                case "TypeView1":
                    RegressionModels.Style = (Style)this.Resources["WindowStyle1"];
                    TypeWindow = "Normal";
                    break;
                case "TypeView2":
                    RegressionModels.Style = (Style)this.Resources["WindowStyle2"];
                    TypeWindow = "Maximized";
                    break;
                default:
                    break;
            }
            MenuItemTypeView();
        }

        /// <summary>
        /// Отображение меню для редактирования таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditingDataTable_Click(object sender, RoutedEventArgs e)
        {
            if (EditingTableON)
            {
                EditingDataTable1.IsChecked = false;
                EditingTableON = false;
                EditingTableOpen = false;
                MainEditingTable.Children.Clear();
                Column[1, 1].Width = new GridLength(0);
                Column[1, 2].Width = new GridLength(0);
                BlockForTable.UpdateLayout();
            }
            else
            {
                EditingDataTable1.IsChecked = true;
                EditingTableON = true;
                Column[1, 1].Width = new GridLength(4);
                Column[1, 2].Width = new GridLength(180);
                MenuForEditingTable();
                BlockForTable.UpdateLayout();
            }
        }

        /// <summary>
        /// Удаление данных из таблицы и удаление таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (KeyDeleteET == 1)
            {
                MessageBoxResult Res = MessageBox.Show("Вы действительно хотите удалить данные?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Res == MessageBoxResult.Yes)
                {
                    for (int i = 1; i <= Row; i++)
                    {
                        for (int j = 1; j <= Col; j++)
                        {
                            DataTable_Cell[i, j].Text = "0";
                            DataTable_Cell[i, j].Foreground = new SolidColorBrush(Color.FromArgb(146, 38, 44, 56));
                            Array[i - 1, j - 1] = 0;
                        }
                    }
                }
            }
            else if (KeyDeleteET == 2)
            {
                MessageBoxResult Res = MessageBox.Show("Вы действительно хотите удалить таблицу?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Res == MessageBoxResult.Yes)
                {
                    Row = 5;
                    Col = 2;
                    FillingTable();
                    UpdateData();
                }
            }
        }

        /// <summary>
        /// Изменение размеров таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Row = Convert.ToInt32(Txt_AmountRows.Text);
                Col = Convert.ToInt32(Txt_AmountColumns.Text);
            }
            catch
            {
                Row = 5;
                Col = 2;
            }

            if (Col < 2)
            {
                Col = 2;
                Txt_AmountColumns.Text = Convert.ToString(Col);
            }
            if (Col > 11)
            {
                Col = 11;
                Txt_AmountColumns.Text = Convert.ToString(Col);
            }
            if (Row < 5)
            {
                Row = 5;
                Txt_AmountRows.Text = Convert.ToString(Row);
            }
            if (Row > 500)
            {
                Row = 500;
                Txt_AmountRows.Text = Convert.ToString(Row);
            }
            UpdateData();
            if (ErrorUpdate == false) FillingTable();
        }

        /// <summary>
        /// Переключение между графиком и таблицей с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_Or_Table_Click(object sender, RoutedEventArgs e)
        {
            Button Choose = (Button)sender;
            switch (Choose.Name)
            {
                case "Table":
                    TypeContent = "Table";
                    break;
                case "Chart":
                    if (Col == 2 && Row > 0) TypeContent = "Chart";
                    else
                    {
                        if (Row <= 0) MessageBox.Show("Данные, на основе которых строится график, отсутствуют.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (Col != 2) MessageBox.Show("Данная программа строит только двумерные графики.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                default:
                    break;
            }
            Template_1();
        }

        /// <summary>
        /// Автосохранение и сохранение размеров окна приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Size_Screen_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Choose = (MenuItem)sender;
            switch (Choose.Name)
            {
                case "AutoSaveSizeScreen":
                    if (AutoSave)
                    {
                        StreamWriter SizeScreen1 = new StreamWriter("Options/SizeScreen.txt");
                        SizeScreen1.WriteLine(WidthScreen);
                        SizeScreen1.WriteLine(HeightScreen);
                        SizeScreen1.Close();
                    }
                    AutoSave = AutoSaveSizeScreen.IsChecked;
                    break;
                case "SaveSizeScreen":
                    StreamWriter SizeScreen2 = new StreamWriter("Options/SizeScreen.txt");
                    SizeScreen2.WriteLine(WidthScreen);
                    SizeScreen2.WriteLine(HeightScreen);
                    SizeScreen2.Close();
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToProgram(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult Res = MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Res == MessageBoxResult.No) e.Cancel = true;
            else
            {
                if (System.IO.Directory.Exists("Options") == false)
                {
                    Directory.CreateDirectory("Options");
                }

                if (AutoSave)
                {
                    StreamWriter SizeScreen = new StreamWriter("Options/SizeScreen.txt");
                    SizeScreen.WriteLine(WidthScreen);
                    SizeScreen.WriteLine(HeightScreen);
                    SizeScreen.Close();
                }

                StreamWriter Options = new StreamWriter("Options/Options.txt");
                Options.WriteLine(TypeWindow);
                Options.WriteLine(AutoSave);
                Options.Close();
            }
        }

        /// <summary>
        /// Выход из приложения при нажатии на крестик в правом верхнем углу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Сохранение архива с данными: исходные данные, график, результат, прогноз
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {
            bool Control = false;
            string PathToFile = "", PathToFolder = "", ErrorMessage = "", Line = TextBox_FilesName[0].Text, _path_ = Environment.CurrentDirectory;

            for (int j = 1; j < 7; j++)
            {
                if (CheckBox_Elements_List[j].IsChecked == true)
                {
                    Control = true;
                    break;
                }
            }

            if (Control)
            {
                Control = false;
                try
                {
                    PathToFolder = _path_ + "\\Save\\" + Line;

                    for (int i = 1; i < Line.Length; i++)
                    {
                        if (Line[i] == ':' && i < (TextBox_FilesName[0].Text.Length - 1))
                        {
                            if (Line[i + 1] == '\\')
                            {
                                PathToFolder = TextBox_FilesName[0].Text;
                            }
                        }
                    }

                    if (System.IO.Directory.Exists(PathToFolder) == true)
                    {
                        MessageBoxResult Res = MessageBox.Show("Папка с именем " + PathToFolder + " уже существует, вы действительно хотите продолжить?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Res == MessageBoxResult.Yes)
                        {
                            Directory.CreateDirectory(PathToFolder);
                            Control = true;
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(PathToFolder);
                        Control = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Не удаётся создать папку с именем " + PathToFolder + ". Возможно, используются символы, которые запрещены при создании папок. Пожалуйста, проверьте название папки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Control = false;
                }

                if (Control)
                {
                    DS.Array = Array;

                    if (CheckBox_Elements_List[1].IsChecked == true)
                    {
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[1].Text;
                        DS.InFile(PathToFile);

                        if (DS.GetError != null)
                        {
                            Control = false;
                            ErrorMessage = "Не удалось создать файл с именем " + TextBox_FilesName[1].Text + ". ";
                        }
                    }

                    if (CheckBox_Elements_List[2].IsChecked == true)
                    {
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[2].Text;
                        DS.InXML(PathToFile);

                        if (DS.GetError != null)
                        {
                            Control = false;
                            ErrorMessage = ErrorMessage + "Не удалось создать файл с именем " + TextBox_FilesName[2].Text + ". ";
                        }
                    }

                    if (CheckBox_Elements_List[3].IsChecked == true)
                    {
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[3].Text;
                        _Chart = DS.Get_Chart_DataSet();
                        DS.Save_Chart(_Chart, PathToFile);

                        if (DS.GetError != null)
                        {
                            Control = false;
                            ErrorMessage = ErrorMessage + "Не удалось создать файл с именем " + TextBox_FilesName[3].Text + ". ";
                        }
                    }

                    if (CheckBox_Elements_List[4].IsChecked == true)
                    {
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[4].Text;
                        _Chart = DS.Get_Chart_Model(RM);
                        DS.Save_Chart(_Chart, PathToFile);

                        if (DS.GetError != null)
                        {
                            Control = false;
                            ErrorMessage = ErrorMessage + "Не удалось создать файл с именем " + TextBox_FilesName[4].Text + ". ";
                        }
                    }

                    if (CheckBox_Elements_List[5].IsChecked == true)
                    {
                        int _Length = _Report.Length;
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[5].Text;
                        PathToFile = System.IO.Path.ChangeExtension(PathToFile, "txt");

                        try
                        {
                            PathToFile = System.IO.Path.ChangeExtension(PathToFile, "txt");
                            StreamWriter File = new StreamWriter(PathToFile);

                            for (int i = 0; i < _Length; i++)
                            {
                                File.WriteLine(_Report[i]);
                            }
                            File.Close();
                        }
                        catch
                        {
                            Control = false;
                            ErrorMessage = ErrorMessage + "Не удалось создать файл с именем " + TextBox_FilesName[5].Text + ". ";
                        }
                    }

                    if (CheckBox_Elements_List[6].IsChecked == true)
                    {
                        int _Length = TextForecast.Length;
                        PathToFile = PathToFolder + "\\" + TextBox_FilesName[6].Text;
                        PathToFile = System.IO.Path.ChangeExtension(PathToFile, "txt");

                        try
                        {
                            PathToFile = System.IO.Path.ChangeExtension(PathToFile, "txt");
                            StreamWriter File = new StreamWriter(PathToFile);

                            for (int i = 0; i < _Length; i++)
                            {
                                File.WriteLine(TextForecast[i]);
                            }
                            File.Close();
                        }
                        catch
                        {
                            Control = false;
                            ErrorMessage = ErrorMessage + "Не удалось создать файл с именем " + TextBox_FilesName[6].Text + ". ";
                        }
                    }

                    if (Control == false)
                    {
                        MessageBox.Show(ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else MessageBox.Show("Данные успешно сохранены в папку " + PathToFolder + ".", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Сохраняемые элементы не выбраны. Не удаётся сохранить данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Отображение контента для работы с прогнозированием
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Prediction_Click(object sender, RoutedEventArgs e)
        {
            string ChooseName = "";
            try
            {
                Button ChooseButton = (Button)sender;
                ChooseName = ChooseButton.Name;
            }
            catch
            {
                try
                {
                    MenuItem ChooseMenuItem = (MenuItem)sender;
                    ChooseName = ChooseMenuItem.Name;
                }
                catch
                {
                    ChooseName = "";
                }
            }

            switch (ChooseName)
            {
                case "InputFactors":
                    TypeContentPrediction = "InputFactors";
                    break;
                case "OutputResult":
                    UpdateValueFactors();
                    if (ErrorUpdateFactors == false)
                    {
                        TextForecast = new string[5];

                        Forecast = RE.Spot_And_Interval_Forecast(Factors);

                        TextForecast[0] = "Результаты прогнозирования";
                        TextForecast[1] = "Уравнение для прогнозирования: " + RM.GetInfoModel["Модель"];
                        TextForecast[2] = "Значения факторов, по которым был построен прогноз: ";
                        for (int i = 0; i < Col - 1; i++)
                        {
                            if (i < Col - 2) TextForecast[2] = TextForecast[2] + "X" + (i + 1) + " = " + Factors[i] + ", ";
                            else TextForecast[2] = TextForecast[2] + "X" + (i + 1) + " = " + Factors[i] + ".";
                        }
                        TextForecast[3] = "Точечный прогноз: " + Forecast["Точечный прогноз"];
                        TextForecast[4] = "Интервальный прогноз: (" + Forecast["Нижняя граница интервального прогноза"] + "; " + Forecast["Верхняя граница интервального прогноза"] + ")";

                        TextPrediction.Text = TextForecast[1] + "\n\n" + TextForecast[2] + "\n\n" + TextForecast[3] + "\n\n" + TextForecast[4];
                        TypeContentPrediction = "OutputResult";
                    }
                    break;
                default:
                    break;
            }
            Template_1();
        }

        /// <summary>
        /// Вызов окна для задания количества знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_Digits_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow_InputNumberDigits InputNumberDigits = new DialogWindow_InputNumberDigits();
            InputNumberDigits.InputDialogWindow.Text = NumberDigits+"";
            InputNumberDigits.Slider_DW.Value = Convert.ToInt32(NumberDigits);
            if (InputNumberDigits.ShowDialog() == true)
            {
                InputNumberDigits.Owner = this;
                NumberDigits = Convert.ToInt32(InputNumberDigits.Input_Text);
                RE = new RegressionEstimation(NumberDigits);
                if (FindingBestModel.IsChecked == false) RE.Estimate(DS, RM);
                else RE.MostSuitableModel(DS, RM);
                
                DataEstimation = RE.GetDataEstimation;
                _Report = RE.GetReport;
                Template_1();
            }
        }

        /// <summary>
        /// Вызов окна для задания параметра сглаживания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Smoothing_Parameter_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow_Input_smoothing_parameter Input_smoothing_parameter = new DialogWindow_Input_smoothing_parameter();
            Input_smoothing_parameter.InputDialogWindow.Text = Smoothing_parameter + "";
            Input_smoothing_parameter.Slider_DW.Value = Convert.ToDouble(Smoothing_parameter);
            if (Input_smoothing_parameter.ShowDialog() == true)
            {
                Input_smoothing_parameter.Owner = this;
                Smoothing_parameter = Convert.ToDouble(Input_smoothing_parameter.Input_Text);
                RM.BuildBrownModel_LinaerTrend(DS, Smoothing_parameter);
                RE.Estimate(DS, RM);

                DataEstimation = RE.GetDataEstimation;
                _Report = RE.GetReport;
                Template_1();
            }
        }

        /// <summary>
        /// Вызов окна для выбора директории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderBrowserDialog_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBox_FilesName[0].Text = folderBrowserDialog1.SelectedPath;
                TextBox_FilesName[0].Foreground = new SolidColorBrush(Colors.Black);
            }
        }

#endregion

#region События и методы

        /// <summary>
        /// Проверка выбранного размера окна приложения
        /// </summary>
        private void MenuItemScreen()
        {
            Screen1.Background = new SolidColorBrush(Colors.Transparent);
            Screen2.Background = new SolidColorBrush(Colors.Transparent);
            Screen3.Background = new SolidColorBrush(Colors.Transparent);
            Screen4.Background = new SolidColorBrush(Colors.Transparent);
            Screen5.Background = new SolidColorBrush(Colors.Transparent);
            Screen1.IsEnabled = true;
            Screen2.IsEnabled = true;
            Screen3.IsEnabled = true;
            Screen4.IsEnabled = true;
            Screen5.IsEnabled = true;
            switch (WidthScreen)
            {
                case 785:
                    if (HeightScreen == 600)
                    {
                        Screen1.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                        Screen1.IsEnabled = false;
                    }
                    break;
                case 850:
                    if (HeightScreen == 600)
                    {
                        Screen2.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                        Screen2.IsEnabled = false;
                    }
                    break;
                case 930:
                    if (HeightScreen == 600)
                    {
                        Screen3.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                        Screen3.IsEnabled = false;
                    }
                    break;
                case 1024:
                    if (HeightScreen == 650)
                    {
                        Screen4.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                        Screen4.IsEnabled = false;
                    }
                    break;
                case 1160:
                    if (HeightScreen == 710)
                    {
                        Screen5.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                        Screen5.IsEnabled = false;
                    }
                    break;
                default:
                    break;
            }
            Screen.Header = HeightScreen + " x " + WidthScreen;
        }
        
        /// <summary>
        /// Проверка выбранного типа окна приложения
        /// </summary>
        private void MenuItemTypeView()
        {
            TypeView1.Background = new SolidColorBrush(Colors.Transparent);
            TypeView2.Background = new SolidColorBrush(Colors.Transparent);
            TypeView1.IsEnabled = true;
            TypeView2.IsEnabled = true;
            switch (TypeWindow)
            {
                case "Normal":
                    TypeView1.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                    TypeView1.IsEnabled = false;
                    TypeView.Header = TypeView1.Header;
                    break;
                case "Maximized":
                    TypeView2.Background = new SolidColorBrush(Color.FromArgb(33, 72, 62, 98));
                    TypeView2.IsEnabled = false;
                    TypeView.Header = TypeView2.Header;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Текущие размеры окна приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SizeScreenChanged(object sender, SizeChangedEventArgs e)
        {
            if(key == false)
            {
                HeightScreen = Convert.ToInt32(RegressionModels.Height);
                WidthScreen = Convert.ToInt32(RegressionModels.Width);
                MenuItemScreen();
            }
        }

        /// <summary>
        /// Выбор одного из трёх блоков меню для задания действий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TCBottomMenu(object sender, SelectionChangedEventArgs e)
        {
            if (TI_DataTable.IsSelected) KeyBottomMenu = 1;
            else if (TI_LoadingData.IsSelected) KeyBottomMenu = 2;
            else if (TI_SaveData.IsSelected) KeyBottomMenu = 3;
            BottomMenu();
        }

        /// <summary>
        /// Выбор типа файла из выпадающего списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox Choose = (ComboBox)sender;
                switch (Choose.Name)
                {
                    case "LoadFile":
                        if (LoadFileTxt.IsSelected) KeyLoadFile = 1;
                        else if (LoadFileXml.IsSelected) KeyLoadFile = 2;
                            break;
                    case "SaveFile":
                        if (SaveFileTxt.IsSelected) KeySaveFile = 1;
                        else if (SaveFileXml.IsSelected) KeySaveFile = 2;
                        break;
                    case "DeleteET":
                        if (DeleteET1.IsSelected) KeyDeleteET = 1;
                        else if (DeleteET2.IsSelected) KeyDeleteET = 2;
                        break;
                    default:
                        break;
                }
            }
            catch
            { 
                KeyLoadFile = 1;
                KeySaveFile = 1;
                KeyDeleteET = 1;
            }
        }

        /// <summary>
        /// Проверка на целочисленный ввод данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged_IntegerNumber(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        /// <summary>
        /// Проверка на вещественный ввод данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged_DoubleNumber(object sender, TextCompositionEventArgs e)
        {
            TextBox Choose = (TextBox)sender;
            string Line = Choose.Text;
            int Check1 = 0, Check2 = 0;

            try
            {
                if (e.Text == ",")
                {
                    for (int i = 0; i < Choose.Text.Length; i++)
                    {
                        if (Line[i] == ',') Check1 += 1;
                    }
                    if (Check1 > 0) e.Handled = true;
                }
                else if(e.Text == "-")
                {
                    for (int i = 0; i < Choose.Text.Length; i++)
                    {
                        if (Line[i] == '-' && i == 0) Check2 += 1;
                        else if (Line[i] == '-' && i != 0) e.Handled = true;
                    }
                    if (Check2 > 0) e.Handled = true;
                }
                else if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
            }
            catch
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Проверка и запрет на ввод пробела
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Запрет на копирование и вставку данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteOrCutCommand(object sender, ExecutedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// Скрытие заполнителя для текстовых полей при сохранении архива с результатами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GotFocusFN(object sender, RoutedEventArgs e)
        {
            int i = 0;
            TextBox Choose = (TextBox)sender;
            switch (Choose.Name)
            {
                case "TextBoxName0":
                    i = 0;
                    break;
                case "TextBoxName1":
                    i = 1;
                    break;
                case "TextBoxName2":
                    i = 2;
                    break;
                case "TextBoxName3":
                    i = 3;
                    break;
                case "TextBoxName4":
                    i = 4;
                    break;
                case "TextBoxName5":
                    i = 5;
                    break;
                case "TextBoxName6":
                    i = 6;
                    break;
                default:
                    break;
            }

            if (TextBox_FilesName[i].Text == default_files_name[i])
            {
                TextBox_FilesName[i].Text = "";
                TextBox_FilesName[i].Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Заполнитель для текстовых полей при сохранении архива с результатами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LostFocusFN(object sender, RoutedEventArgs e)
        {
            int i = 0;
            TextBox Choose = (TextBox)sender;
            switch (Choose.Name)
            {
                case "TextBoxName0":
                    i = 0;
                    break;
                case "TextBoxName1":
                    i = 1;
                    break;
                case "TextBoxName2":
                    i = 2;
                    break;
                case "TextBoxName3":
                    i = 3;
                    break;
                case "TextBoxName4":
                    i = 4;
                    break;
                case "TextBoxName5":
                    i = 5;
                    break;
                case "TextBoxName6":
                    i = 6;
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(TextBox_FilesName[i].Text))
            {
                TextBox_FilesName[i].Text = default_files_name[i];
                TextBox_FilesName[i].Foreground = new SolidColorBrush(Color.FromArgb(146, 38, 44, 56));
            }
        }

        /// <summary>
        /// Скрытие заполнителя для ячейки таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GotFocusTableCell(object sender, RoutedEventArgs e)
        {
            TextBox Choose = (TextBox)sender;

            if (Choose.Text == "0")
            {
                Choose.Text = "";
                Choose.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Заполнитель для ячейки таблицы, когда данные в ней отсутствуют
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LostFocusTableCell(object sender, RoutedEventArgs e)
        {
            TextBox Choose = (TextBox)sender;

            if (string.IsNullOrEmpty(Choose.Text))
            {
                Choose.Text = "0";
                Choose.Foreground = new SolidColorBrush(Color.FromArgb(146, 38, 44, 56));
            }
        }

        /// <summary>
        /// Проверка выбранных элементов для сохранения архива с результатами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckElements_SaveResults(object sender, RoutedEventArgs e)
        {
            int i = 0;
            CheckBox Choose = (CheckBox)sender;
            if (Choose.Name == "Check1")
            {
                if (Choose.IsChecked == false)
                {
                    CheckBox_Elements_List[1].IsChecked = false;
                    CheckBox_Elements_List[2].IsChecked = false;
                    CheckBox_Elements_List[3].IsChecked = false;
                    CheckBox_Elements_List[4].IsChecked = false;
                    CheckBox_Elements_List[5].IsChecked = false;
                    CheckBox_Elements_List[6].IsChecked = false;
                }
                else if (Choose.IsChecked == true)
                {
                    CheckBox_Elements_List[1].IsChecked = true;
                    CheckBox_Elements_List[2].IsChecked = true;
                    if (Col != 2)
                    {
                        CheckBox_Elements_List[3].IsChecked = false;
                        CheckBox_Elements_List[4].IsChecked = false;
                    }
                    else
                    {
                        CheckBox_Elements_List[3].IsChecked = true;
                        CheckBox_Elements_List[4].IsChecked = true;
                    }
                    CheckBox_Elements_List[5].IsChecked = true;
                    if(CheckBox_Elements_List[6].IsEnabled) CheckBox_Elements_List[6].IsChecked = true;
                }
            }
            else
            {
                switch (Choose.Name)
                {
                    case "Check2":
                        i = 1;
                        break;
                    case "Check3":
                        i = 2;
                        break;
                    case "Check4":
                        i = 3;
                        break;
                    case "Check5":
                        i = 4;
                        break;
                    case "Check6":
                        i = 5;
                        break;
                    case "Check7":
                        i = 6;
                        break;
                    default:
                        break;
                }
            }

            if (CheckBox_Elements_List[i].IsChecked == false) CheckBox_Elements_List[0].IsChecked = false;
            else
            {
                for (int j = 1; j < 7; j++)
                {
                    if (CheckBox_Elements_List[j].IsChecked == true)
                    { 
                        CheckBox_Elements_List[0].IsChecked = true;
                    }
                    else
                    {
                        if (j == 6 && CheckBox_Elements_List[j].IsEnabled == false) CheckBox_Elements_List[0].IsChecked = true;
                        else if (Col != 2 && (j == 3 || j == 4)) CheckBox_Elements_List[0].IsChecked = true;
                        else
                        {
                            CheckBox_Elements_List[0].IsChecked = false;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Масштабирование для графика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _slider.Value = Math.Round(_slider.Value, 1);
            ValueSlider.Text = Convert.ToString(_slider.Value);
            _Chart.LayoutTransform = new ScaleTransform(_slider.Value, _slider.Value);
        }

#endregion   
        
    }
}