import { baseUrl } from 'api/http'
import React, { useEffect, useState } from 'react'
import { GiBroadsword, GiPocketBow } from 'react-icons/gi'
import { useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { ToCamelCase } from '../../helpers/stringHelper'
import { Hero } from '../../models/Hero'
import { IRootState } from '../../store/store'
import '../../styles/App.scss'
import { attributes } from '../../styles/attributes'
import './HeroList.scss'

interface IHeroInList {
    hero: Hero
}

const light = 'rgba(255,255,255,0.9)'
const dark = 'rgba(0,0,0,0.8)'

const HeroInList: React.FC<IHeroInList> = ({ hero }) => {
    const [isHovering, setIsHovering] = useState(false)
    const [normalizedUrl, setNormalizedUrl] = useState('')

    const themeMode = useSelector<IRootState, Palette>((state) => state.palette)
    let navigate = useNavigate()

    useEffect(() => {
        setNormalizedUrl(
            hero?.imageLink
                ?.replace(/\\/g, '/')
                .replace(/"/g, '')
                .replace(/&quot;/g, '')
        )
    }, [hero?.imageLink])

    function addOrRedirectToHero() {
        let urlHeroName = ToCamelCase(hero?.name)
        navigate(`../hero/${urlHeroName}`)
    }

    function renderAttackType() {
        return (
            <div className="attack-type">
                {hero?.attackType === 'Melee' ? (
                    <GiBroadsword />
                ) : (
                    <GiPocketBow />
                )}
            </div>
        )
    }

    function renderMainAttribute() {
        return (
            <div
                className="main-attribute"
                style={{
                    color: attributes.find(
                        (attribute) => attribute.name === hero?.mainAttribute
                    ).color,
                }}
            >
                {hero.mainAttribute}
            </div>
        )
    }

    function renderTags() {
        return (
            <div className="d-flex justify-content-between tag">
                <div>
                    {hero.roles?.map((tag) => (
                        <span className="badge bg-secondary mx-1" key={tag}>
                            {tag}
                        </span>
                    ))}
                </div>
            </div>
        )
    }

    function renderHeroContainer() {
        return (
            <div>
                <div className="d-flex justify-content-between w-100">
                    <div>
                        <div className="hero-name">{hero.name}</div>
                        <div className="fix-padding d-flex justify-content-start">
                            {renderAttackType()}
                            {renderMainAttribute()}
                        </div>
                        {renderTags()}
                    </div>
                </div>
            </div>
        )
    }

    return (
        <div className="hero-container">
            {hero && (
                <div
                    className={
                        isHovering !== (themeMode === 'dark')
                            ? 'hero hero-selected'
                            : 'hero hero-default'
                    }
                    onClick={addOrRedirectToHero}
                    style={{
                        backgroundImage: `url(${baseUrl}${normalizedUrl}),
                        ${
                            isHovering !== (themeMode === 'dark')
                                ? `linear-gradient(90deg, ${dark}, rgba(0,0,0,0.2))`
                                : `linear-gradient(90deg, ${light}, 50%, rgba(0,0,0,0.2) 70%)`
                        }`,
                    }}
                    onMouseOver={() => setIsHovering(true)}
                    onMouseOut={() => setIsHovering(false)}
                >
                    <div className="container-padding">
                        {renderHeroContainer()}
                    </div>
                </div>
            )}
        </div>
    )
}

export default HeroInList
