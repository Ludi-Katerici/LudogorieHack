function initializeCities() {
    const selectCities = document.querySelector("#postalCode");
    const selectProvinces = document.querySelector("#provinces");
    function onClickHandler() {
        const province = selectProvinces.value;
        let cities;
        fetch(`/Cities/${province}`)
            .then(x => x.json())
            .then(result => {
                let cities = result.sort((cityA, cityB) => cityA.postalCode - cityB.postalCode)
                selectCities.innerHTML = "";
                for (const city of cities) {
                    selectCities.innerHTML += `<option value="${city.id}">${city.name + ", " + city.postalCode}</option>`;
                }
            })
            .catch(x => {selectCities.innerHTML = "";});
    }

    selectProvinces.onchange = onClickHandler;
}