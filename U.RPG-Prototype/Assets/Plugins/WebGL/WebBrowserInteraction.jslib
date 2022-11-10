/*
 * WebBrowserInteraction.jslib - A WebGL Plugin that allows JavaScript functionality to be invoked from C#
 * Created by : Allan N. Murillo
 * Last Edited : 2/28/2022
 */

var WebBrowserInteraction =
{
    InitializeJsLib: function ()
    {
        console.log('[JS_LIB]: Web Browser Interaction Initializing');
        FS.mkdir('/GameData');
        FS.mount(IDBFS, {}, '/GameData');
        FS.syncfs(true, function (err)
        {
            assert(!err);
            console.log('[JS_LIB]: Loading Save Settings from Indexed Database');
            unityInstance.SendMessage('PersistentGameManager', 'LoadSettingsFromIndexedDb');
        });                          
    },
    WindowFullscreen : function()
    {
        unityInstance.SetFullscreen(1);
    },
    CancelFullscreen : function()
    {
        unityInstance.SetFullscreen(0);
    },
    LostFocus : function(hasFocus)
    {
        if(hasFocus){
            unityInstance.SendMessage('PersistentGameManager', 'RaiseResume');
        }
        else{
            unityInstance.SendMessage('PersistentGameManager', 'RaiseLimitedPause');
        }
    },
    OpenNewTab : function(url)
    {
        url = UTF8ToString(url);
        window.open(url, 'blank');
    },
    Save : function()
    {
        //console.log('[JS_LIB]: Saving Settings to Indexed Database');
        FS.syncfs(false, function (err)
        {
            assert(!err);

            if(err){
                console.log('[JS_LIB]: Save to Indexed Database has Failed');
            }
            else{
                console.log('[JS_LIB]: Save to Indexed Database was Successful');
            }
        });
    },
    QuitCleanup : function()
    {
        console.log('[JS_LIB]: quit the runtime and clean up the memory used by the Unity instance');
        unityInstance.Quit().then(onQuit);
    },    
    onQuit : function()
    {
        console.log('[JS_LIB]: onQuit');
        unityInstance = null;
    }
}
mergeInto(LibraryManager.library, WebBrowserInteraction);
