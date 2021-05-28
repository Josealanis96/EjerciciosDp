<?php
if(isset($_GET['jsonBtn']))
{
	getJson();
}

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

	$json_final = json_encode(array_values($lista_json));

	echo "<script>console.log($json_final)</script>";

	file_put_contents("Respuesta1.json", $json_final);
}
?>

<form method="get">
	<input type="submit" name="jsonBtn" value="Imprimir respuesta Json por consola y generar archivo Json">
</form>