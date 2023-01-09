import { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import {
  getAllSpecies,
  getAllLocations,
  Species,
  Location,
} from "../../clients/apiClient";
import "./BrowseSightingsControls.scss";

interface BrowseSightingsContolsProps {
  query: URLSearchParams;
  setQuery: (params: URLSearchParams) => void;
  isShowingMap: boolean;
  setIsShowingMap: (toggle: boolean) => void;
}

interface Filters {
  species: Species[];
  locations: Location[];
  // date: DateFilter;
}

interface DateFilter {
  startDate: Date;
  endDate: Date;
}

export const BrowseSightingsControls = ({
  query,
  setQuery,
  isShowingMap,
  setIsShowingMap,
}: BrowseSightingsContolsProps) => {
  const history = useHistory();

  const [species, setSpecies] = useState<Species[]>();
  const [locations, setLocations] = useState<Location[]>();
  const [filtersToApply, setFiltersToApply] = useState<Filters>({
    species: [],
    locations: [],
  });

  useEffect(() => {
    getAllSpecies().then((allSpecies) => setSpecies(allSpecies));
    getAllLocations().then((allLocations) => setLocations(allLocations));
  }, []);

  const addFilter = (param: Location | Species) => {
    const newFiltersToApply = { ...filtersToApply };
    if ("species" in param && !filtersToApply.locations.includes(param)) {
      //location
      newFiltersToApply.locations.push(param);
    } else if (
      "locations" in param &&
      !filtersToApply.species.includes(param)
    ) {
      //species
      newFiltersToApply.species.push(param);
    }
    setFiltersToApply(newFiltersToApply);
  };

  const removeFilter = (param: Location | Species) => {
    const newFiltersToApply = { ...filtersToApply };
    if ("species" in param) {
      //location
      newFiltersToApply.locations = newFiltersToApply.locations.filter(
        (l) => l.id !== param.id
      );
    } else if ("locations" in param) {
      //species
      newFiltersToApply.species = newFiltersToApply.species.filter(
        (s) => s.id !== param.id
      );
    }
    setFiltersToApply(newFiltersToApply);
  };

  const applyFilters = () => {
    history.push(`/sightings?${getQueryString()}`);
  };

  const getQueryString = () => {
    const newQuery = new URLSearchParams();
    filtersToApply.locations.forEach((location) =>
      newQuery.append("locationid", `${location.id}`)
    );
    filtersToApply.species.forEach((species) =>
      newQuery.append("speciesid", `${species.id}`)
    );
    setQuery(newQuery);
    return newQuery.toString();
  };

  return (
    <>
      <button className="filter-button">Filters</button>
      <div className="filter-container">
        <div className="location-filter">
          <ol>
            {filtersToApply.locations.map((location) => {
              return (
                <li key={location.id} className="selected-location">
                  <p>{location.description}</p>
                  <button onClick={() => removeFilter(location)}> x </button>
                </li>
              );
            })}
          </ol>

          {locations ? (
            <ol className="locations-list">
              {locations.map((location) => {
                return (
                  <li key={location.id} onClick={() => addFilter(location)}>
                    {location.description}
                  </li>
                );
              })}
            </ol>
          ) : (
            <p> Loading... </p>
          )}
        </div>

        <div className="species-filter">
          <ol>
            {filtersToApply.species.map((species) => {
              return (
                <li key={species.id} className="selected-species">
                  <p>{species.name}</p>
                  <button onClick={() => removeFilter(species)}> x </button>
                </li>
              );
            })}
          </ol>
          {species ? (
            <ol>
              {species.map((species) => {
                return (
                  <li key={species.id} onClick={() => addFilter(species)}>
                    {species.name}
                  </li>
                );
              })}
            </ol>
          ) : (
            <p> Loading... </p>
          )}
        </div>

        <button onClick={() => applyFilters()}>Apply</button>
      </div>

      <button
        className="map-button"
        onClick={() => {
          setIsShowingMap(!isShowingMap);
        }}
      >
        {isShowingMap ? "List View" : "Map View"}
      </button>
    </>
  );
};
