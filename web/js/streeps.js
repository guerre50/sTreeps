var config = {
	width: 360, 
	height: 600,
	params: { 
		enableDebugging:"0",
		backgroundcolor: "A0A0A0",
		bordercolor: "000000",
		textcolor: "FFFFFF",
		logoimage: "logo.png",
		progressbarimage: "MyProgressFrame2.png",
		progressframeimage: "MyProgressFrame.png"
	}
};
config.params["disableContextMenu"] = true;
var u = new UnityObject2(config);

jQuery(function() {
	var $missingScreen = jQuery("#unityPlayer").find(".missing");
	var $brokenScreen = jQuery("#unityPlayer").find(".broken");
	$missingScreen.hide();
	$brokenScreen.hide();
	u.observeProgress(function (progress) {
		switch(progress.pluginStatus) {
			case "broken":
				$brokenScreen.find("a").click(function (e) {
					e.stopPropagation();
					e.preventDefault();
					u.installPlugin();
					return false;
				});
				$brokenScreen.show();
			break;
			case "missing":
				$missingScreen.find("a").click(function (e) {
					e.stopPropagation();
					e.preventDefault();
					u.installPlugin();
					return false;
				});
				$missingScreen.show();
			break;
			case "installed":
				$missingScreen.remove();
			break;
			case "first":
				document.goo.HideTitle();
				jQuery("#unityPlayer").toggleClass("loaded");
			break;
		}
	});
	u.initPlugin(jQuery("#unityPlayer")[0], "streeps.unity3d");

	var $ = jQuery;
	var button = $("#button");
	var timeout;
	button.on("click", function() {
		button.toggleClass("clicked", true);
		clearTimeout(timeout);
		timeout = setTimeout(function() { button.removeClass("clicked");}, 500);
		u.getUnity().SendMessage("StripController", "Shake", "MixStrips");
	})

});

function StartPlay() {
	document.goo.ShowTitle();
	jQuery("#button").addClass("visible");
}