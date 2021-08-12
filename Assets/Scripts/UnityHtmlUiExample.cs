using UnityEngine;
using Vuplex.WebView;
using Vuplex.WebView.Demos;

class UnityHtmlUiExample : MonoBehaviour {

    CanvasWebViewPrefab _canvasWebViewPrefab;
    HardwareKeyboardListener _hardwareKeyboardListener;

    async void Start() {

        // The CanvasWebViewPrefab's `InitialUrl` property is set via the editor, so it
        // will automatically initialize itself with that URL.
        _canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();

        _setUpKeyboard();
        // https://developer.vuplex.com/webview/WebViewPrefab#WaitUntilInitialized
        await _canvasWebViewPrefab.WaitUntilInitialized();

        // Listen for messages from JavaScript.
        // https://developer.vuplex.com/webview/IWebView#MessageEmitted
        // https://support.vuplex.com/articles/how-to-send-messages-from-javascript-to-c-sharp
        _canvasWebViewPrefab.WebView.MessageEmitted += (sender, eventArgs) => {
            var message = eventArgs.Value;
            if (message.StartsWith("auth_success")) {
                var authToken = message.Split(new char[] {':'}, 2)[1];
                Debug.Log("Signin succeeded! Auth token: " + authToken);
            } else if (message.StartsWith("auth_skipped")) {
                Debug.Log("Signin skipped");
            }
        };
    }

    void _setUpKeyboard() {

        // Send keys from the hardware keyboard to the webview.
        _hardwareKeyboardListener = HardwareKeyboardListener.Instantiate();
        _hardwareKeyboardListener.InputReceived += (sender, eventArgs) => {
            // Include key modifiers if the webview supports them.
            var webViewWithKeyModifiers = _canvasWebViewPrefab.WebView as IWithKeyModifiers;
            if (webViewWithKeyModifiers == null) {
                _canvasWebViewPrefab.WebView.HandleKeyboardInput(eventArgs.Value);
            } else {
                webViewWithKeyModifiers.HandleKeyboardInput(eventArgs.Value, eventArgs.Modifiers);
            }
        };
    }
}
