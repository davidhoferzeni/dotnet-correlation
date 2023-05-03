<h1 class="text-s">Welcome to SvelteKit</h1>
<script>
	const fetchImage = (async () => {
		const response = await fetch('https://dog.ceo/api/breeds/image/random')
    return await response.json()
	})()
</script>

<div class="flex flex-row">

<div class="flex flex-col">
    <label class="text-3xl" for="quantity">Quantity (between 1 and 5):</label>
    <input type="number" id="weatherId" min="1" step="1">
</div>

{#await fetchImage}
	<p>...waiting</p>
{:then data}
	<img src={data.message} alt="Dog image" />
{:catch error}
	<p>An error occurred!</p>
{/await}

</div>
