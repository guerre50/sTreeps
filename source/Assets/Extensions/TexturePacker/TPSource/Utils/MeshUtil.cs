using UnityEngine;

public class MeshUtil {

public static Mesh createBox(Vector2 size, Vector2 zero, Rect textureCoords) {
	Vector3[] vertices = {
	new Vector3(0, 0, 0), // 1 ___ 2
	new Vector3(0, size.y, 0), // | |
	new Vector3(size.x, size.y, 0),// | |
	new Vector3(size.x, 0, 0) // 0 —-- 3
	};
	for (int i = 0; i < vertices.Length; i++) {
	vertices[i].x -= zero.x;
	vertices[i].y -= zero.y;
	}
	Vector2[] uv = {
	new Vector2(textureCoords.xMin, 1 - textureCoords.yMax),
	new Vector2(textureCoords.xMin, 1 - textureCoords.yMin),
	new Vector2(textureCoords.xMax, 1 - textureCoords.yMin),
	new Vector2(textureCoords.xMax, 1 - textureCoords.yMax)
	};
	int[] triangles = {
	0, 1, 2,
	0, 2, 3
	};
	Mesh _return = new Mesh();
	_return.vertices = vertices;
	_return.uv = uv;
	_return.triangles = triangles;
		
	Vector3[] normals = {
		new Vector3(0, 0, -1), // 1 ___ 2
		new Vector3(0, 0, -1), // | |
		new Vector3(0, 0, -1),// | |
		new Vector3(0, 0, -1) // 0 —-- 3
	};
	
	_return.normals = normals;
		
	return _return;
}
}