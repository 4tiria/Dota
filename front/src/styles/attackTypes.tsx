import { AttackType } from "models/Hero";
import React from "react";
import { IconType } from "react-icons";
import { GiBroadsword, GiPocketBow } from "react-icons/gi";

export const attackTypes: {name: AttackType | "All", icon: IconType | any }[] = [
    {name: AttackType.Melee, icon: <GiBroadsword/>},
    {name: AttackType.Ranged, icon: <GiPocketBow/>},
];