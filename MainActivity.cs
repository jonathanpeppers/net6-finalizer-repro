using Java.Interop;
using static System.Diagnostics.Debug;

namespace net6_finalizer_repro
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            for (int i = 0 ; i < 4; i++)
                Dispose_Finalized (i);
        }

        public void Dispose_Finalized (int i)
        {
            var d = false;
            var f = false;
            FinalizerHelpers.PerformNoPinAction (() => {
                var v     = new JavaDisposedObject (() => d = true, () => f = true);
                GC.KeepAlive (v);
            });
            JniEnvironment.Runtime.ValueManager.CollectPeers ();
            GC.WaitForPendingFinalizers ();
            JniEnvironment.Runtime.ValueManager.CollectPeers ();
            GC.WaitForPendingFinalizers ();
            Console.WriteLine ($"Actual Values: i={i}, d={d}, f={f}");
            Assert (!d, "d should be false");
            Assert (f, "f should be true");
        }
    }

    [JniTypeSignature ("java/lang/Object")]
    class JavaDisposedObject : JavaObject {

        public Action   OnDisposed;
        public Action   OnFinalized;

        public JavaDisposedObject (Action disposed, Action finalized)
        {
            OnDisposed  = disposed;
            OnFinalized = finalized;
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing)
                OnDisposed ();
            else
                OnFinalized ();
        }
    }
}