if ( ! Detector.webgl ) Detector.addGetWebGLMessage();

var camera, controls, scene, renderer;
var plane, cube;
var mouse, raycaster, isShiftDown = false;

var rollOverMesh, rollOverMaterial;
var cubeGeo, cubeMaterial;

var currentTool;

var objects = [];

init();
gameLoop();

function init() {

	camera = new THREE.PerspectiveCamera( 45, window.innerWidth / window.innerHeight, 1, 10000 );
	camera.position.set( 0, 800, 1300 );
	camera.lookAt( new THREE.Vector3() );

	controls = new THREE.OrbitControls( camera );
	controls.damping = 0.2;

	scene = new THREE.Scene();

	// roll-over helpers

	rollOverGeo = new THREE.BoxGeometry( 50, 50, 50 );
	rollOverMaterial = new THREE.MeshBasicMaterial( { color: 0xff0000, opacity: 0.5, transparent: true } );
	rollOverMesh = new THREE.Mesh( rollOverGeo, rollOverMaterial );
	scene.add( rollOverMesh );

	// cubes

	cubeGeo = new THREE.BoxGeometry( 50, 50, 50 );
	cubeMaterial = new THREE.MeshLambertMaterial( { color: 0xFFAEC9, shading: THREE.FlatShading, overdraw: 1 } );

	// grid

	var size = 500, step = 50;

	var geometry = new THREE.Geometry();

	for ( var i = - size; i <= size; i += step ) {

		geometry.vertices.push( new THREE.Vector3( - size, 0, i ) );
		geometry.vertices.push( new THREE.Vector3(   size, 0, i ) );

		geometry.vertices.push( new THREE.Vector3( i, 0, - size ) );
		geometry.vertices.push( new THREE.Vector3( i, 0,   size ) );

	}

	var material = new THREE.LineBasicMaterial( { color: 0x000000, opacity: 0.2, transparent: true } );

	var line = new THREE.Line( geometry, material, THREE.LinePieces );
	scene.add( line );

	//

	raycaster = new THREE.Raycaster();
	mouse = new THREE.Vector2();

	var geometry = new THREE.PlaneBufferGeometry( 1000, 1000 );
	geometry.applyMatrix( new THREE.Matrix4().makeRotationX( - Math.PI / 2 ) );

	plane = new THREE.Mesh( geometry );
	plane.visible = false;
	scene.add( plane );

	objects.push( plane );

	// Lights

	var ambientLight = new THREE.AmbientLight( 0x606060 );
	scene.add( ambientLight );

	var directionalLight = new THREE.DirectionalLight( 0xffffff );
	directionalLight.position.set( 1, 0.75, 0.5 ).normalize();
	scene.add( directionalLight );

	renderer = new THREE.WebGLRenderer( { antialias: true } );
	renderer.setClearColor( 0xf0f0f0 );
	renderer.setPixelRatio( window.devicePixelRatio );
	renderer.setSize( window.innerWidth, window.innerHeight );

	var container = document.getElementById('content');
	container.appendChild( renderer.domElement );

	document.addEventListener( 'mousemove', onDocumentMouseMove, false );
	document.addEventListener( 'mousedown', onDocumentMouseDown, false );
	document.addEventListener( 'keydown', onDocumentKeyDown, false );
	document.addEventListener( 'keyup', onDocumentKeyUp, false );

	//

	window.addEventListener( 'resize', onWindowResize, false );


	// UI

	$('#tools label').click(function() {
		selectTool($(this).attr('for'));
	});

	selectTool('pencil');
	rollOverMesh.visible = false;
}

function onWindowResize() {

	camera.aspect = window.innerWidth / window.innerHeight;
	camera.updateProjectionMatrix();

	renderer.setSize( window.innerWidth, window.innerHeight );

}

function onDocumentMouseMove( event ) {

	event.preventDefault();

	mouse.set( ( event.clientX / window.innerWidth ) * 2 - 1, - ( event.clientY / window.innerHeight ) * 2 + 1 );

	raycaster.setFromCamera( mouse, camera );

	var intersects = raycaster.intersectObjects( objects );

	if ( currentTool === 'pencil' && intersects.length > 0 ) {

		var intersect = intersects[ 0 ];

		rollOverMesh.position.copy( intersect.point ).add( intersect.face.normal );
		rollOverMesh.position.divideScalar( 50 ).floor().multiplyScalar( 50 ).addScalar( 25 );

		rollOverMesh.visible = true;
	} else {
		rollOverMesh.visible = false;
	}

}

function onDocumentMouseDown( event ) {

	event.preventDefault();

	mouse.set( ( event.clientX / window.innerWidth ) * 2 - 1, - ( event.clientY / window.innerHeight ) * 2 + 1 );

	raycaster.setFromCamera( mouse, camera );

	var intersects = raycaster.intersectObjects( objects );

	if ( currentTool === 'pencil' && intersects.length > 0 ) {

		var intersect = intersects[ 0 ];

		// delete cube

		if ( isShiftDown ) {

			if ( intersect.object != plane ) {

				scene.remove( intersect.object );

				objects.splice( objects.indexOf( intersect.object ), 1 );

			}

		// create cube

		} else {

			var voxel = new THREE.Mesh( cubeGeo, cubeMaterial );
			voxel.position.copy( intersect.point ).add( intersect.face.normal );
			voxel.position.divideScalar( 50 ).floor().multiplyScalar( 50 ).addScalar( 25 );
			scene.add( voxel );

			objects.push( voxel );

		}

	}

}

function onDocumentKeyDown( event ) {
	console.log(event.keyCode);

	switch( event.keyCode ) {

		case 16:
			isShiftDown = true;
			break;

		case 86:
			selectTool('pointer');
			break;

		case 66:
			selectTool('pencil');
			break;

	}

}

function onDocumentKeyUp( event ) {

	switch ( event.keyCode ) {

		case 16: isShiftDown = false; break;

	}

}

function selectTool(str) {
	$('#'+str).next().addClass('selected').siblings().removeClass('selected');

	currentTool = str;

	if(currentTool === 'pointer') {
		controls.enabled = true;

		if(rollOverMesh.visible) {
			rollOverMesh.visible = false;
		}
	} else {
		controls.enabled = false;
	}
}


function gameLoop() {
	requestAnimationFrame( gameLoop );

	controls.update();

	renderer.render(scene, camera);
};