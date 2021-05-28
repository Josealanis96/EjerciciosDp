<?php

//Condicional para manejar el click del botón y ejecutar el metodo getJson.
if(isset($_GET['jsonBtn']))
{
	getJson();
}

/*Funcion para conseguir una respuesta Json y verificar qué objetos Json tienen una propiedad con color distinto a "green",
  eliminando del arreglo las que cumplan esta condicional*/. 
function getJson()
{
	$url_rest_api = "https://my-json-server.typicode.com/dp-danielortiz/dptest_jsonplaceholder/items";
	$respuesta_json = file_get_contents($url_rest_api);
	$lista_json = json_decode($respuesta_json);
	$total_objetos = count($lista_json);
	for($i = 0; $i<$total_objetos;$i++)
	{
		if($lista_json[$i]->color !== "green")
		{
			unset($lista_json[$i]);
		}		
	}

	//Se codifica el arreglo final a Json, se imprime en consola y se exporta a un archivo .json.
	$json_final = json_encode(array_values($lista_json), JSON_PRETTY_PRINT);

	echo "<script>console.log($json_final)</script>";

	file_put_contents("Respuesta1.json", $json_final);
}
?>

<title>RestApi#1</title>

<form method="get">
	<input type="submit" name="jsonBtn" value="Imprimir respuesta Json por consola y generar archivo Json">
</form>