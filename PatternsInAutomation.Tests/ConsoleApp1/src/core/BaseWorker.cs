using System;
using System.ComponentModel;
using System.Security;
using System.Threading;
using AutoDataVPBank.src;
using OpenQA.Selenium;

namespace AutoDataVPBank.core
{
    public abstract class BaseWorker
    {
        private static readonly object SyncLock = new object();
        private int _total = 1;
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
                Library.CancelTokenSrc = new CancellationTokenSource();
                try
                {
                    Process();
                }
                catch (Exception ex)
                {
                    //TODO: Fail when not exist.
                    Library.Logg.Error(ex.Message);
                    Library.KillProcess();
                }

                _total++;
            }
            catch (ArgumentException ae)
            {
                Library.Logg.Error(ae.Message);

            }
            catch (ThreadInterruptedException tie)
            {
                Library.Logg.Error(tie.Message);

            }
            catch (SecurityException se)
            {
                Library.Logg.Error(se.Message);

            }
            catch (Exception ex)
            {
                Library.Logg.Error(ex.Message);

            }
            finally
            {
                Monitor.Exit(SyncLock);

                Library.Logg.Info("---------End One------------");
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
                Library.MForm.btnRun.Enabled = true;
                Library.MForm.btnRun.Text = @"Start";
            }
            catch (Exception exception)
            {
                Library.Logg.Error(exception.Message);
                throw;
            }
        }

        private void _RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                Library.MForm.lb_process_status.Text = @"Task Cancelled.";
            }
            else if (e.Error != null)
            {
                Library.MForm.lb_process_status.Text = @"Error while performing background operation.";
            }
            else
            {
                Library.MForm.lb_process_status.Text = @"Task Completed...";
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
                Library.IsRun = true;
                Setup();
                Library.JobName = GetType().Name;
                // Signal the shutdown event
                Library.ShutdownEvent.Reset();
                // Make sure to resume any paused threads
                Library.PauseEvent.Set();
                _resetEvent.Set();
                //Start the async operation here
                if (MoWorker.IsBusy)
                {
                    //Stop/Cancel the async operation here
                    //_mOWorker.CancelAsync();
                    //_resetEvent.Set();
                    //_resetEvent.WaitOne(); // will block until _resetEvent.Set() call made
                    Library.MForm.btnRun.Enabled = false;
                    Library.MForm.lb_process_status.Text = @"Cancelling...";

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    MoWorker.CancelAsync();
                }
                else
                {
                    Library.MForm.btnRun.Text = @"Stop";
                    Library.MForm.lb_process_status.Text = @"Running...";

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
                Library.Logg.Error(e.Message);
                throw;
            }
        }

        public static void Stop()
        {
            try
            {
                Library.IsRun = false;
                // Signal the shutdown event
                Library.ShutdownEvent.Set();
                // Make sure to resume any paused threads
                Library.PauseEvent.Set();
                Library.CancelTokenSrc.Cancel();
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
                    Library.MForm.btnRun.Enabled = false;
                    Library.MForm.lb_process_status.Text = @"Cancelling...";

                    // Notify the worker thread that a cancel has been requested.
                    // The cancel will not actually happen until the thread in the
                    // DoWork checks the bwAsync.CancellationPending flag, for this
                    // reason we set the label to "Cancelling...", because we haven't
                    // actually cancelled yet.
                    MoWorker.CancelAsync();
                }
                else
                {
                    Library.MForm.btnRun.Text = @"Stop";
                    Library.MForm.lb_process_status.Text = @"Running...";

                    // Kickoff the worker thread to begin it's DoWork function.
                    MoWorker.RunWorkerAsync();
                }
            }
        }
    }
}
