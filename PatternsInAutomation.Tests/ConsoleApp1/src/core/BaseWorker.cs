using System;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using static AutoDataVPBank.Library;

namespace AutoDataVPBank.core
{
    public abstract class BaseWorker
    {
        private static readonly object SyncLock = new object();
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
        protected static BackgroundWorker MoWorker;

        protected abstract void Process();
        protected virtual bool GetInfor()
        {
            return true;
        }
        public abstract void Teardown();

        private void Setup()
        {
            //Create BackgroundWorker
            MoWorker = new BackgroundWorker();
            MoWorker.DoWork += _DoWork;
            MoWorker.ProgressChanged += _ProgressChanged;
            MoWorker.RunWorkerCompleted += _RunWorkerCompleted;
            MoWorker.WorkerReportsProgress = true;
            MoWorker.WorkerSupportsCancellation = true;
        }

        private void Perform()
        {
            Monitor.Enter(SyncLock);
            try
            {
                MoWorker.ReportProgress(0);
                CancelTokenSrc = new CancellationTokenSource();
                try
                {
                    Process();
                }
                catch (Exception ex)
                {
                    //TODO: Fail when not exist.
                    Logg.Error(ex.Message);
                    KillProcess();
                }
            }
            catch (ArgumentException ae)
            {
                Logg.Error(ae.Message);

            }
            catch (ThreadInterruptedException tie)
            {
                Logg.Error(tie.Message);

            }
            catch (SecurityException se)
            {
                Logg.Error(se.Message);

            }
            catch (Exception ex)
            {
                Logg.Error(ex.Message);

            }
            finally
            {
                Monitor.Exit(SyncLock);

                Logg.Info("---------End One------------");
            }
        }
        
        //BackGroundWorker:
        private void _DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // The sender is the BackgroundWorker object we need it to
                // report progress and check for cancellation.
                var bwAsync = sender as BackgroundWorker;
                while (!e.Cancel)
                {
                    // do something
                    Perform();
                    if (bwAsync != null && !bwAsync.CancellationPending) continue;
                    // Set the e.Cancel flag so that the WorkerCompleted event
                    // knows that the process was cancelled.
                    e.Cancel = true;

                }

                bwAsync?.ReportProgress(100);
                _resetEvent.Set(); // signal that worker is done

                //Set again Status button here:
                MForm.btnRun.Invoke((MethodInvoker)delegate
                {
                    MForm.btnRun.Enabled = true;
                    MForm.btnRun.Text = @"Start";
                });
                
            }
            catch (Exception exception)
            {
                Logg.Error(exception.Message);
                throw;
            }
        }

        private void _RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                MForm.lb_process_status.Text = @"Task Cancelled.";
            }
            else if (e.Error != null)
            {
                MForm.lb_process_status.Text = @"Error while performing background operation.";
            }
            else
            {
                MForm.lb_process_status.Text = @"Task Completed...";
            }
        }

        private void _ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Start()
        {
            try
            {
                IsRun = true;
                Setup();
                JobName = GetType().Name;
                // Signal the shutdown event
                ShutdownEvent.Reset();
                // Make sure to resume any paused threads
                PauseEvent.Set();
                _resetEvent.Set();
                //Start the async operation here
                if (MoWorker.IsBusy)
                {
                    //Stop/Cancel the async operation here
                    //_mOWorker.CancelAsync();
                    //_resetEvent.Set();
                    //_resetEvent.WaitOne(); // will block until _resetEvent.Set() call made
                    MForm.btnRun.Invoke((MethodInvoker)delegate
                    {
                        MForm.btnRun.Enabled = false;
                    });
                    MForm.lb_process_status.Invoke((MethodInvoker)delegate
                    {
                        MForm.lb_process_status.Text = @"Cancelling...";
                    });
                    

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    MoWorker.CancelAsync();
                }
                else
                {
                    MForm.btnRun.Invoke((MethodInvoker)delegate
                    {
                        MForm.btnRun.Text = @"Stop";
                    });
                    MForm.lb_process_status.Invoke((MethodInvoker)delegate
                    {
                        MForm.lb_process_status.Text = @"Running...";
                    });
                    

                    // Kickoff the worker thread to begin it's DoWork function.
                    MoWorker.RunWorkerAsync();
                }

                //Library.MainForm.btnRun.Text = @"Stop";
                //_mOWorker.RunWorkerAsync();
                //_resetEvent.WaitOne();
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        public static void Stop()
        {
            try
            {
                IsRun = false;
                // Signal the shutdown event
                ShutdownEvent.Set();
                // Make sure to resume any paused threads
                PauseEvent.Set();
                CancelTokenSrc.Cancel();
                DriverFactory.StopBrowser();
            }
            catch (WebDriverException)
            {

            }
            catch (NullReferenceException)
            {

            }
            catch (InvalidOperationException)
            {

            }
            catch (ArgumentException)
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (MoWorker.IsBusy)
                {
                    //Stop/Cancel the async operation here
                    //_mOWorker.CancelAsync();
                    //_resetEvent.Set();
                    //_resetEvent.WaitOne(); // will block until _resetEvent.Set() call made
                    MForm.btnRun.Invoke((MethodInvoker)delegate
                    {
                        MForm.btnRun.Enabled = false;
                    });
                    MForm.lb_process_status.Invoke((MethodInvoker)delegate
                    {
                        MForm.lb_process_status.Text = @"Cancelling...";
                    });

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    MoWorker.CancelAsync();
                }
                else
                {
                    MForm.btnRun.Invoke((MethodInvoker)delegate
                    {
                        MForm.btnRun.Text = @"Stop";
                    });
                    MForm.lb_process_status.Invoke((MethodInvoker)delegate
                    {
                        MForm.lb_process_status.Text = @"Running...";
                    });
                    

                    // Kickoff the worker thread to begin it's DoWork function.
                    MoWorker.RunWorkerAsync();
                }
            }
        }
    }
}
