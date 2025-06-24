import React from 'react'
import { Hero } from '../../models/Hero'

interface IHeroRoles {
    hero: Hero
}

const HeroTags: React.FC<IHeroRoles> = ({ hero }) => {
    return (
        <div className="hero-tags">
            {hero?.roles.map((tag) => {
                return (
                    <div className="btn btn-sm hero-tag hero-tag-own" key={tag}>
                        {tag}
                    </div>
                )
            })}
        </div>
    )
}

export default HeroTags
