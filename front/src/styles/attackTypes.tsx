import { AttackType } from 'models/Hero'
import React from 'react'
import { IconType } from 'react-icons'
import { GiBroadsword, GiPocketBow } from 'react-icons/gi'

export const attackTypes: { name: AttackType | 'All'; icon: IconType | any }[] =
    [
        { name: 'Melee', icon: <GiBroadsword /> },
        { name: 'Ranged', icon: <GiPocketBow /> },
    ]
