# FPM13 Procedural Material Notes

Unity-generated maps use layered Perlin noise for brushed metal directionality, black pitting, recess soot, cavity occlusion, warm edge wear, and restrained verdigris. Normal maps are derived from height; ORM uses R=occlusion, G=roughness, B=metallic. The render proof applies these materials to simple gear-key and pipe-bundle proxy forms under warm grazing light, rather than flat swatches.
