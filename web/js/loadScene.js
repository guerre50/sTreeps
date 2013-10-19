require([
	'goo/entities/GooRunner',
	'goo/loaders/DynamicLoader',
	'goo/math/Vector3',

	'goo/renderer/Camera',
	'goo/entities/components/CameraComponent',

	'goo/entities/components/ScriptComponent',
	'goo/scripts/OrbitCamControlScript',

	'goo/renderer/light/DirectionalLight',
	'goo/entities/components/LightComponent',
	'goo/util/GameUtils'

], function (
	GooRunner,
	DynamicLoader,
	Vector3,

	Camera,
	CameraComponent,

	ScriptComponent,
	OrbitCamControlScript,

	DirectionalLight,
	LightComponent,
	GameUtils
) {
	'use strict';
	var rootNode;
	var goo;
	var $ = jQuery;
	var cameraEntity;

	function init() {
		goo = new GooRunner({
			antialias: true
		});
		goo.renderer.domElement.id = 'goo';
		goo.renderer.setClearColor(253/255.0, 255/255.0, 253/255.0, 0);
		jQuery("#logo").append(goo.renderer.domElement);
		document.goo = {
			ShowTitle: ShowTitle,
			HideTitle: HideTitle,
			Time: Time
		};

		// The Loader takes care of loading data from a URL...
		var loader = new DynamicLoader({
			world: goo.world, 
			rootPath: './scene/'
		});
		loader.load('Project.project')
		.then(function(entities) {
			goo.world.process();
			InitCamera();
			$(window).resize(WindowResize);
			rootNode = goo.world.getManager("EntityManager").getEntityByName("logoAnimation2/entities/RootNode.entity");
		}).then(null, function(e) {
			alert('Failed to load scene: ' + e);
		});
	}

	function InitCamera() {
		var camera = new Camera(45, 1.2134595163, 0.1, 14.0413069725);
		cameraEntity = goo.world.createEntity('ViewCameraEntity');
		var cameraComponent = new CameraComponent(camera);

		cameraEntity.setComponent(cameraComponent);
		camera.projectionMode = 1;
		camera.setFrustum(0.1, 14.0413069725, -2, 2, 1.5, -1.5);
		cameraEntity.addToWorld();
	    cameraEntity.transformComponent.setTranslation(0, 2, 0).lookAt(new Vector3(0, 0, 0), Vector3.UNIT_Z.mul(-1));
	}

	function Time() {
		return goo.world.time;
	}

	function WindowResize() {
		window.camera = cameraEntity;
	}

	function ShowTitle() {
		rootNode.setComponent(new ScriptComponent({
			run: function(entity) {
	            entity.transformComponent.transform.translation.lerp(new Vector3(0, 0, 1.05), goo.world.tpf*1.8);
	            entity.transformComponent.setUpdated();
	        }
		}));
	}

	function HideTitle() {
		rootNode.setComponent(new ScriptComponent({
			run: function(entity) {
	            entity.transformComponent.transform.translation.lerp(new Vector3(0, 0, 4), goo.world.tpf*2.3);
	            entity.transformComponent.setUpdated();
	        }
		}));
	}

	init();
});
