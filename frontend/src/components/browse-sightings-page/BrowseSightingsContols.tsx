import { useEffect, useState } from "react";
import {
  getAllSpecies,
  getAllLocations,
  Species,
  Location,
} from "../../clients/apiClient";
import "./BrowseSightingsControls.scss";

interface BrowseSightingsContolsProps {
  isShowingMap: boolean;
  setIsShowingMap: (toggle: boolean) => void;
}

interface Filters {
  species: Species[];
  locations: Location[];
}

export const BrowseSightingsControls = ({
  isShowingMap,
  setIsShowingMap,
}: BrowseSightingsContolsProps) => {
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

  return (
    <>
      <button id="filter-button" className="button">
        Filter
      </button>
      <div id="filter-container" className="filters">
        <div className="location-filter">
          {locations ? (
            <ol className="locations-list">
              {locations.map((location) => {
                return <li key={location.id}>{location.description}</li>;
              })}
            </ol>
          ) : (
            <p> Loading... </p>
          )}
        </div>

        <div className="species-filter">
          {species ? (
            <ol>
              {species.map((species) => {
                return <li key={species.id}>{species.name}</li>;
              })}
            </ol>
          ) : (
            <p> Loading... </p>
          )}
        </div>
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
