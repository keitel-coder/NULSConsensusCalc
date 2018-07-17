using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NULSCalc
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 代理人受托押金下限
        /// </summary>
        private const int EntrustedDown = 0;

        /// <summary>
        /// 代理人受托押金上限
        /// </summary>
        private const int EntrustedUp = 500000;

        /// <summary>
        /// 委托人押金下限
        /// </summary>
        private const int EntrustDown = 2000;

        /// <summary>
        /// 代理人佣金比例下限
        /// </summary>
        private const int AgentCommissionDown = 0;

        /// <summary>
        /// 代理人佣金上限
        /// </summary>
        private const int AgentCommissionUp = 20;

        /// <summary>
        /// 代理人押金下限
        /// </summary>
        private const int NodeDepositDown = 20000;

        private void DockHead_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();//实现整个窗口的拖动
        }

        /// <summary>
        /// 官网标签点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelWebSite_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://nuls.io");
        }

        /// <summary>
        /// 中文论坛
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelChineseBBS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.nuls.org.cn");

        }

        /// <summary>
        /// 电报群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelTelegram_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://t.me/nulsio");
        }

        private void LabelBinance_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.binance.com/trade.html?symbol=NULS_BTC");
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelMini_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Topmost = false;
            this.WindowState = WindowState.Minimized;
            return;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 选择代理共识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadionType_Agent_Checked(object sender, RoutedEventArgs e)
        {
            if (GroupBoxCommision != null)
            {
                GroupBoxCommision.Visibility = Visibility.Visible;
                StackPanelAgentdeposit.Visibility = Visibility.Visible;
                this.Height = this.Height + GroupBoxCommision.ActualHeight;
                this.StackPanel_Entrust.Visibility = Visibility.Collapsed;
                this.StackPanel_Entrusted.Visibility = Visibility.Visible;
            }
            ClearIncomeTextBox();
        }

        /// <summary>
        /// 选择委托共识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadionType_Entrust_Checked(object sender, RoutedEventArgs e)
        {
            if (GroupBoxCommision != null)
            {
                var height = this.Height - GroupBoxCommision.ActualHeight;
                GroupBoxCommision.Visibility = Visibility.Collapsed;
                StackPanelAgentdeposit.Visibility = Visibility.Collapsed;
                this.Height = height;
                this.StackPanel_Entrust.Visibility = Visibility.Visible;
                this.StackPanel_Entrusted.Visibility = Visibility.Collapsed;
            }
            ClearIncomeTextBox();
        }

        /// <summary>
        /// 刷新共识信息
        /// </summary>
        private bool RefreshConsensusInfo()
        {
            var consensusInfo = GetNulsNodeInfo.GetConsensusInfo();
            if (consensusInfo == null)
            {
                return false;
            };
            this.Label_AgentCount.Dispatcher.Invoke(new Action(delegate
            {
                this.Label_AgentCount.Content = consensusInfo.agentCount.ToString();
            }));
            this.Label_TotalDeposit.Dispatcher.Invoke(new Action(delegate
            {
                this.Label_TotalDeposit.Content = (consensusInfo.totalDeposit / 100000000.0).ToString();
            }));
            this.Label_RewardOfDay.Dispatcher.Invoke(new Action(delegate
            {
                this.Label_RewardOfDay.Content = (consensusInfo.rewardOfDay / 100000000.0).ToString();
            }));
            this.Label_ConsensusAccountNumber.Dispatcher.Invoke(new Action(delegate
            {
                this.Label_ConsensusAccountNumber.Content = consensusInfo.consensusAccountNumber.ToString();
            }));
            return true;
        }

        /// <summary>
        /// 刷新区块信息
        /// </summary>
        /// <returns></returns>
        private bool RefreshBlockInfo()
        {
            var blockInfo = GetNulsNodeInfo.GetBlockInfo();
            if (blockInfo == null || blockInfo.list == null || blockInfo.list.Length == 0)
            {
                return false;
            };
            this.LabelBlockHeight.Dispatcher.Invoke(new Action(delegate
            {
                this.LabelBlockHeight.Content = blockInfo.list[0].height.ToString();
            }));
            this.LabelBlockTime.Dispatcher.Invoke(new Action(delegate
            {
                this.LabelBlockTime.Content = Uitls.DateTimeUitls.GetDateTime(blockInfo.list[0].createTime, true).ToString("yyyy-MM-dd HH:mm:ss");
            }));
            return true;
        }

        /// <summary>
        /// 定时器执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RefreshConsensusInfo();
            RefreshBlockInfo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool resfresh = RefreshConsensusInfo();
            //刷新失败了
            if (!resfresh)
            {

            }
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = 3000;//执行间隔时间,单位为毫秒  
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
        }

        /// <summary>
        /// 区块高度连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelBlockHeight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://explorer.nuls.io/");
        }

        /// <summary>
        /// 代理押金
        /// </summary>
        private decimal NodeDeposit
        {
            get
            {
                //代理押金
                decimal nodeDeposit;
                if (!decimal.TryParse(TextBox_NodeDeposit.Text, out nodeDeposit))
                {
                    return -1;
                }
                return nodeDeposit;
            }
        }

        /// <summary>
        /// 受委托金额
        /// </summary>
        private decimal Entrusted
        {
            get
            {
                //委托金额
                decimal entrusted;
                if (!decimal.TryParse(TextBox_Entrusted.Text, out entrusted))
                {
                    return -1;
                }
                return entrusted;

            }
        }

        /// <summary>
        /// 委托金额
        /// </summary>
        private decimal Entrust
        {
            get
            {
                //委托金额
                decimal entrust;
                if (!decimal.TryParse(TextBox_Entrust.Text, out entrust))
                {
                    return -1;
                }
                return entrust;

            }
        }

        /// <summary>
        /// 是否代理节点
        /// </summary>
        public bool IsAgent
        {
            get
            {
                return RadionType_Agent.IsChecked.HasValue && RadionType_Agent.IsChecked.Value;
            }
        }

        /// <summary>
        /// 委托金额
        /// </summary>
        private decimal AgentCommission
        {
            get
            {
                //委托金额
                decimal agentCommission;
                if (!decimal.TryParse(TextBox_AgentCommission.Text, out agentCommission))
                {
                    return -1;
                }
                return agentCommission;
            }
        }

        /// <summary>
        /// 委托金额
        /// </summary>
        private decimal TotalDeposit
        {
            get
            {
                if (Label_TotalDeposit.Content == null) return -1;
                //委托金额
                decimal totalDeposit;
                if (!decimal.TryParse(Label_TotalDeposit.Content.ToString(), out totalDeposit))
                {
                    return -1;
                }
                return totalDeposit;
            }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        private string ErrorMessage
        {
            set
            {
                Label_ErrorMessage.Content = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    Label_ErrorMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Label_ErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 计算收益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_CalcIncome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CheckTextBox()) { return; }

            decimal agentCommission = AgentCommission;
            int totalToken = 100000000;
            //总委托金额
            decimal totalDeposit = TotalDeposit;

            //代理委托
            if (RadionType_Agent.IsChecked.Value)
            {
                //代理押金
                decimal nodeDeposit = NodeDeposit;
                if (nodeDeposit < 20000) return;
                //代理收益
                decimal agentIncome = CalcAgentIncome(totalToken, totalDeposit, nodeDeposit);
                //委托佣金收益计算
                decimal commissionIncome = 0;
                //受委托金额
                decimal entrusted = Entrusted;
                if (agentCommission > 0 && agentCommission > 0)
                {
                    agentCommission = agentCommission / 100.0M;
                    commissionIncome = CalcAgentCommissionIncome(totalToken, totalDeposit, agentCommission, entrusted);
                }
                //总收益
                decimal totalIncomeOfDay = agentIncome + commissionIncome;
                //总收益率
                decimal totalIncomeRateOfDay = (totalIncomeOfDay / nodeDeposit) * 100;
                ///佣金收益率
                decimal commissionIncomeRateOfDay = (commissionIncome / nodeDeposit) * 100;
                //总收益显示
                TextBox_AgentIncomeOfDay.Text = totalIncomeOfDay.ToString("0.########");
                TextBox_AgentIncomeRateOfDay.Text = totalIncomeRateOfDay.ToString("0.########");
                TextBox_AgentIncomeOfMonth.Text = (totalIncomeOfDay * 30).ToString("0.########");
                TextBox_AgentIncomeRateOfMonth.Text = (totalIncomeRateOfDay * 30).ToString("0.########");
                TextBox_AgentIncomeOfYear.Text = (totalIncomeOfDay * 365).ToString("0.########");
                TextBox_AgentIncomeRateOfYear.Text = (totalIncomeRateOfDay * 365).ToString("0.########");
                //佣金显示
                TextBox_CommisionIncomeOfDay.Text = commissionIncome.ToString("0.########");
                TextBox_CommisionIncomeRateOfDay.Text = commissionIncomeRateOfDay.ToString("0.########");
                TextBox_CommisionIncomeOfMonth.Text = (commissionIncome * 30).ToString("0.########");
                TextBox_CommisionIncomeRateOfMonth.Text = (commissionIncomeRateOfDay * 30).ToString("0.########");
                TextBox_CommisionIncomeOfYear.Text = (commissionIncome * 365).ToString("0.########");
                TextBox_CommisionIncomeRateOfYear.Text = (commissionIncomeRateOfDay * 365).ToString("0.########");
            }
            else
            {
                decimal entrust = Entrust;
                //代理佣金比例
                if (agentCommission < 0 || agentCommission > 50)
                {
                    return;
                }
                agentCommission = agentCommission / 100.0M;
                //共识委托
                var totalIncomeOfDay = CalcEntrusIncome(totalToken, totalDeposit, agentCommission, entrust);
                decimal totalIncomeRateOfDay = (totalIncomeOfDay / entrust) * 100;
                //佣金显示
                TextBox_AgentIncomeOfDay.Text = totalIncomeOfDay.ToString("0.########");
                TextBox_AgentIncomeRateOfDay.Text = totalIncomeRateOfDay.ToString("0.########");
                TextBox_AgentIncomeOfMonth.Text = (totalIncomeOfDay * 30).ToString("0.########");
                TextBox_AgentIncomeRateOfMonth.Text = (totalIncomeRateOfDay * 30).ToString("0.########");
                TextBox_AgentIncomeOfYear.Text = (totalIncomeOfDay * 365).ToString("0.########");
                TextBox_AgentIncomeRateOfYear.Text = (totalIncomeRateOfDay * 365).ToString("0.########");
            }
        }

        /// <summary>
        /// 清除收益框
        /// </summary>
        private void ClearIncomeTextBox()
        {
            if (TextBox_AgentIncomeOfDay != null)
            {
                TextBox_AgentIncomeOfDay.Text =
                TextBox_AgentIncomeRateOfDay.Text =
                TextBox_AgentIncomeOfMonth.Text =
                TextBox_AgentIncomeRateOfMonth.Text =
                TextBox_AgentIncomeOfYear.Text =
                TextBox_AgentIncomeRateOfYear.Text =
                TextBox_CommisionIncomeOfDay.Text =
                TextBox_CommisionIncomeRateOfDay.Text =
                TextBox_CommisionIncomeOfMonth.Text =
                TextBox_CommisionIncomeRateOfMonth.Text =
                TextBox_CommisionIncomeOfYear.Text =
                TextBox_CommisionIncomeRateOfYear.Text = string.Empty;
                ErrorMessage = string.Empty;
            }
        }

        /// <summary>
        /// 计算代理节点收益
        /// </summary>
        /// <param name="totalToken">代币总量</param>
        /// <param name="totalDeposit">总委托押金</param>
        /// <param name="nodeDeposit">节点抵押金</param>
        /// <param name="agentCommission"></param>
        /// <param name="entrust"></param>
        /// <returns></returns>
        private decimal CalcAgentIncome(int totalToken, decimal totalDeposit, decimal nodeDeposit)
        {
            //代理收益
            return nodeDeposit * ((totalToken * 0.05M / 365) / totalDeposit);
        }

        /// <summary>
        /// 计算代理节点佣金收益
        /// </summary>
        private decimal CalcAgentCommissionIncome(int totalToken, decimal totalDeposit, decimal agentCommission, decimal entrust)
        {
            return CalcAgentIncome(totalToken, totalDeposit, entrust) * agentCommission;
        }

        /// <summary>
        /// 计算委托收益
        /// </summary>
        /// <param name="totalToken">代币总量</param>
        /// <param name="totalDeposit">总委托押金</param>
        /// <param name="agentCommission">委托佣金</param>
        /// <param name="entrust">委托金</param>
        /// <returns></returns>
        private decimal CalcEntrusIncome(int totalToken, decimal totalDeposit, decimal agentCommission, decimal entrust)
        {
            return CalcAgentIncome(totalToken, totalDeposit, entrust) * (1 - agentCommission);
        }

        /// <summary>
        /// 文本获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        private Brush nomarlBrush = new SolidColorBrush(new Color()
        {
            R = 101,
            G = 142,
            B = 199,
            A = 255,
        });

        /// <summary>
        /// 警告色
        /// </summary>
        private Brush WarnBrush = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// 代理押金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_NodeDeposit_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isAgent = IsAgent;
            if (!isAgent) return;
            //代理人受托押金
            decimal nodeDeposit = NodeDeposit;
            if (nodeDeposit < NodeDepositDown)
            {
                ErrorMessage = "代理人押金下限为20000，快去买NULS吧";
                TextBox_NodeDeposit.BorderBrush = WarnBrush;
            }
            else
            {
                TextBox_NodeDeposit.BorderBrush = nomarlBrush;
            }
        }

        /// <summary>
        /// 代理佣金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_AgentCommission_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isAgent = IsAgent;
            //代理节点时，且代理押金超过20万，可以不填
            decimal agentCommission = AgentCommission;
            if (agentCommission < AgentCommissionDown || agentCommission > AgentCommissionUp)
            {
                ErrorMessage = "代理人佣金比例为0-20%";
                TextBox_AgentCommission.BorderBrush = WarnBrush;
            }
            else
            {
                TextBox_AgentCommission.BorderBrush = nomarlBrush;
            }
        }

        /// <summary>
        /// 受托押金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Entrusted_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isAgent = IsAgent;
            //不是代理不用填受托押金
            if (!isAgent) return;
            //代理节点时，且代理押金超过20万，可以不填
            decimal entrusted = Entrusted;
            //委托金额
            if (entrusted < EntrustedDown)
            {
                ErrorMessage = "代理人受托押金下限为" + EntrustedDown;
                TextBox_AgentCommission.BorderBrush = WarnBrush;
            }
            else if (entrusted > EntrustedUp)
            {
                ErrorMessage = "代理人受托押金上限为" + EntrustedUp;
                TextBox_AgentCommission.BorderBrush = WarnBrush;
            }
            else
            {
                TextBox_AgentCommission.BorderBrush = nomarlBrush;
            }
        }

        /// <summary>
        /// 委托押金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Entrust_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isAgent = IsAgent;
            if (isAgent) return;
            decimal entrust = Entrust;
            //委托金额
            if (entrust < EntrustDown)
            {
                ErrorMessage = $"委托人押金下限为{EntrustDown}，快去买NULS吧";
                TextBox_Entrust.BorderBrush = WarnBrush;
            }
            else
            {
                TextBox_Entrust.BorderBrush = nomarlBrush;
            }
        }

        /// <summary>
        /// 检查输入
        /// </summary>
        /// <returns></returns>
        private bool CheckTextBox()
        {
            if (TotalDeposit < 0)
            {
                ErrorMessage = "网络故障，获取共识信息失败";
                return false;
            }
            bool isAgent = IsAgent, noError = true;
            //代理节点时，且代理押金超过20万，可以不填
            decimal nodeDeposit = NodeDeposit, entrusted = Entrusted, entrust = Entrust, agentCommission = AgentCommission;
            if (isAgent)
            {
                if (nodeDeposit < NodeDepositDown)
                {
                    ErrorMessage = "代理人押金下限为20000，快去买NULS吧";
                    TextBox_NodeDeposit.BorderBrush = WarnBrush;
                    noError = false;
                }
                else
                {
                    TextBox_NodeDeposit.BorderBrush = nomarlBrush;
                }
                //委托金额
                if (entrusted < EntrustedDown)
                {
                    TextBox_AgentCommission.BorderBrush = WarnBrush;
                    if (noError)
                    {
                        ErrorMessage = "代理人受托押金下限为20万";
                    }
                    noError = false;
                }
                else if (entrusted > EntrustedUp)
                {
                    TextBox_AgentCommission.BorderBrush = WarnBrush;
                    if (noError)
                    {
                        ErrorMessage = "代理人受托押金上限为50万";
                    }
                    noError = false;
                }
                else
                {
                    TextBox_AgentCommission.BorderBrush = nomarlBrush;
                }
            }
            else
            {
                //委托金额
                if (entrust < EntrustDown)
                {
                    TextBox_Entrust.BorderBrush = WarnBrush;
                    if (noError)
                    {
                        ErrorMessage = "委托人押金下限为2000，快去买NULS吧";
                    }
                    noError = false;
                }
                else
                {
                    TextBox_Entrust.BorderBrush = nomarlBrush;
                }
            }
            if (agentCommission < AgentCommissionDown || agentCommission > AgentCommissionUp)
            {
                TextBox_AgentCommission.BorderBrush = WarnBrush;
                if (noError)
                {
                    ErrorMessage = "代理人佣金比例为0-20%";
                }
                noError = false;
            }
            else
            {
                TextBox_AgentCommission.BorderBrush = nomarlBrush;
            }
            if (noError)
            {
                ErrorMessage = string.Empty;
            }
            return noError;
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelAbout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("开发者：Keitel，QQ：549322842");
        }
    }
}