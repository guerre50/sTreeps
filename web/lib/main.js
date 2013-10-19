var gooRenderer;
require(['goo'], function() {
    require([
        'goo/entities/GooRunner',
        'goo/renderer/Camera',
        'goo/entities/components/CameraComponent',
        'goo/shapes/ShapeCreator',
        'goo/entities/EntityUtils',
        'goo/renderer/shaders/ShaderLib',
        'goo/renderer/Material',
        'goo/entities/components/MeshRendererComponent',
        'goo/renderer/light/PointLight',
        'goo/entities/components/LightComponent',
    ], function(
        GooRunner,
        Camera,
        CameraComponent,
        ShapeCreator,
        EntityUtils,
        ShaderLib,
        Material,
        MeshRendererComponent,
        PointLight,
        LightComponent
    ) {
        var goo = new GooRunner();
        document.getElementById("gooDiv").appendChild(goo.renderer.domElement);

        // Add box
        var meshData = ShapeCreator.createBox(1, 1, 1);
        var boxEntity = EntityUtils.createTypicalEntity(goo.world, meshData);
        var material = Material.createMaterial(ShaderLib.texturedLit, 'BoxMaterial');
		boxEntity.meshRendererComponent.materials.push(material);
		boxEntity.addToWorld();


    	// Add light
    	var light = new PointLight();
		var lightEntity = goo.world.createEntity('light');
		lightEntity.setComponent(new LightComponent(light));
		lightEntity.transformComponent.transform.translation.set(0, 3, 3);
		lightEntity.addToWorld();

    	// Add camera
        var camera = new Camera(35, 1, 0.1, 1000);
        var cameraEntity = goo.world.createEntity('Camera');

		cameraEntity.setComponent(new CameraComponent(camera));
		cameraEntity.transformComponent.transform.translation.set(0, 0, 5);
		cameraEntity.addToWorld();


		gooRenderer = this;
    });
});