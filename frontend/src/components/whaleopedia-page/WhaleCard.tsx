import React from "react";
import { Species } from "../../clients/apiClient";
import { Card } from "../card/Card";
import "./WhaleCard.scss";

interface WhaleCardProps {
  whale: Species;
}

export const WhaleCard: React.FC<WhaleCardProps> = ({ whale }) => {
  return (
    <Card title={whale.name} imageUrl={whale.photoUrl}>
      <div className="whale-card__scientific">
        Scientific Name: {whale.scientificName}
      </div>
      <div className="whale-card__description">
        Description: {whale.description}
      </div>
    </Card>
  );
};
