import { Box, Paper } from '@mui/material'
import React, { useEffect, useState } from 'react'
import 'react-bootstrap'
import Modal from 'react-bootstrap/Modal'
import { useSelector } from 'react-redux'
import { useNavigate, useParams } from 'react-router-dom'
import { deleteHero, getById, getByName, updateHero } from '../../api/heroApi'
import '../../app/styles/App.scss'
import { HeroImageSize } from '../../globalConstants'
import { User } from '../../models/dto/User'
import { AttackType, Hero, MainAttribute } from '../../models/Hero'
import { IRootState } from '../../store/store'
import HeroAttackType from './HeroAttackType'
import HeroAttribute from './HeroAttribute'
import HeroImage from './HeroImage'
import './HeroInfo.scss'
import HeroName from './HeroName'
import HeroTags from './HeroTags'

import CheckOutlinedIcon from '@mui/icons-material/CheckOutlined'
import ClearOutlinedIcon from '@mui/icons-material/ClearOutlined'
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined'
import EditOutlinedIcon from '@mui/icons-material/EditOutlined'

export const HeroInfo = () => {
    const params = useParams()
    const navigate = useNavigate()
    const [hero, setHero] = useState<Hero>()
    const [editMode, setEditMode] = useState(false)
    const [deleteDialogVisible, setDeleteDialogVisible] = useState(false)

    const [heroName, setHeroName] = useState('')
    const [heroAttackType, setHeroAttackType] = useState<AttackType>()
    const [heroAttribute, setHeroAttribute] = useState<MainAttribute>()
    const [tags, setHeroTags] = useState<string[]>([])

    const user = useSelector<IRootState, User>((state) => state.user)

    useEffect(() => {
        if (/\d/.test(params.name)) {
            let number = parseInt(params.name)
            if (isNaN(number)) {
                //todo: create hero `${}` don't exist
            } else {
                getById(number).then((response) => {
                    setHero(response)
                })
            }
        } else {
            getByName(params.name).then((response) => {
                setHero(response)
            })
        }
    }, [])

    function startEdit() {
        setEditMode(true)
        setHeroName(hero.name)
        setHeroTags(hero.roles)
        setHeroAttribute(hero.mainAttribute)
        setHeroAttackType(hero.attackType)
    }

    function confirm() {
        let flag = true
        setHero((prevState) => {
            const newState = {
                ...prevState,
                name: heroName,
                mainAttribute: heroAttribute,
                attackType: heroAttackType,
                tags: tags,
            }
            if (flag) {
                flag = false
                updateHero(newState).then(() => {
                    setEditMode(false)
                })
            }

            return newState
        })
    }

    function cancel() {
        setEditMode(false)
    }

    function callDeleteHero() {
        deleteHero(hero).then(() => navigate('../../heroList'))
    }

    function renderHeroName() {
        return (
            <HeroName
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroName(value)}
            />
        )
    }

    function renderHeroAttackType() {
        return (
            <HeroAttackType
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroAttackType(value)}
            />
        )
    }

    function renderHeroAttribute() {
        return (
            <HeroAttribute
                hero={hero}
                editMode={editMode}
                callBackFunction={(value) => setHeroAttribute(value)}
            />
        )
    }

    function renderHeroTags() {
        return <HeroTags hero={hero} />
    }

    function renderHeroImage() {
        return <HeroImage hero={hero} editMode={editMode} />
    }

    function renderDeleteDialog() {
        return (
            <Modal show={deleteDialogVisible} onHide={() => {}}>
                <Modal.Dialog>
                    <div>
                        <Modal.Header closeButton>
                            <Modal.Title>
                                Вы точно хотите удалить героя?
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Footer>{deleteDialogFooter}</Modal.Footer>
                    </div>
                </Modal.Dialog>
            </Modal>
        )
    }

    const deleteDialogFooter = (
        <div>
            <div className="btn btn-outline-danger" onClick={callDeleteHero}>
                Удалить
            </div>
            <div
                className="btn btn-outline-secondary"
                onClick={() => setDeleteDialogVisible(false)}
            >
                Отмена
            </div>
        </div>
    )

    return (
        <div className="hero-container">
            <Box>
                <div className="hero-edit-panel">
                    {user.accessLevel === 'Admin' ? (
                        <>
                            {editMode ? (
                                <div className="d-flex justify-content-center">
                                    <div
                                        className="btn custom-icon"
                                        onClick={confirm}
                                    >
                                        <CheckOutlinedIcon className="tick" />
                                    </div>
                                    <div
                                        className="btn custom-icon"
                                        onClick={cancel}
                                    >
                                        <ClearOutlinedIcon className="cross" />
                                    </div>
                                </div>
                            ) : (
                                <div className="d-flex justify-content-center">
                                    {renderDeleteDialog()}
                                    <div
                                        className="btn custom-icon"
                                        onClick={startEdit}
                                    >
                                        <EditOutlinedIcon />
                                    </div>
                                    <div
                                        className="btn custom-icon"
                                        onClick={() =>
                                            setDeleteDialogVisible(true)
                                        }
                                    >
                                        <DeleteOutlineOutlinedIcon />
                                    </div>
                                </div>
                            )}
                        </>
                    ) : (
                        <></>
                    )}
                </div>
                <Paper className="hero-info">
                    <div className="d-flex justify-content-between">
                        <Paper
                            style={{
                                width: HeroImageSize.full.width,
                                height: HeroImageSize.full.height,
                            }}
                        >
                            {renderHeroImage()}
                        </Paper>
                        <div className="hero-header">
                            {!!hero ? (
                                <>
                                    {renderHeroName()}
                                    <div className="d-flex justify-content-center">
                                        {renderHeroAttackType()}
                                        {renderHeroAttribute()}
                                    </div>
                                    {renderHeroTags()}
                                </>
                            ) : (
                                <></>
                            )}
                            <hr />
                        </div>
                    </div>
                    <div className="hero-description"></div>
                </Paper>
            </Box>
        </div>
    )
}

export default HeroInfo
