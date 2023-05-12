<script>
	const delay = (/** @type {number} */ ms) => new Promise(res => setTimeout(res, ms));
	const weatherFetchRequest = (async () => {
		await delay(500);
		const response = await fetch(`http://127.0.0.1:5296/weatherforecast/${numberInput}`)
		console.log(response.headers.get('x-correlation-id'));
    return response;
	});

	let numberInput = 3;
	$: if (numberInput > 0) {
		fetchImage = weatherFetchRequest();
	}
	let fetchImage = weatherFetchRequest();


</script>

<div class="flex flex-col items-center justify-center p-8 gap-8 min-h-screen">

	<label class="text-3xl " for="quantity">Enter an Id for a Weather:</label>
	<input class="text-3xl text-center shadow appearance-none border rounded 
	w-28 py-4 px-4 text-gray-700 mb-3 leading-tight 
	focus:outline-none focus:shadow-outline" 
	bind:value={numberInput}
	type="number" 
	id="weatherId"
	min="1" 
	max="10" 
	step="1">

	{#await fetchImage}
		<p>...loading data</p>
	{:then data}
		{#await data.text()}
			<p>...</p>
			{:then text}
			{#if data.status === 200}
			<p class="text-3xl">Weather with id {numberInput} is... {text}</p>
			{:else}
			<div class="flex flex-col items-center justify-center p-8 gap-2">
				<p class="text-3xl">Error:</p>
				<p class="text-3xl">{text} - {data.status}</p>
				<p class="text-3xl">Correlation id: {data.headers.get('x-correlation-id')}</p>
			</div>
			{/if}	
		{/await}
	{:catch error}
		<p>An error occurred!</p>
	{/await}

</div>
