using UnityEngine;
using Vuplex.WebView;

class UnityHtmlUiExample : MonoBehaviour {

    CanvasWebViewPrefab _canvasWebViewPrefab;

    async void Start() {

        // Get a reference to the CanvasWebViewPrefab.
        // https://support.vuplex.com/articles/how-to-reference-a-webview
        // The CanvasWebViewPrefab's `InitialUrl` property is set via the editor, so it
        // will automatically initialize itself with that URL.
        _canvasWebViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();

        // Wait for the prefab to initialize because its WebView property is null until then.
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
}
