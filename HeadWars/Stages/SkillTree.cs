using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HeadWars
{
	// The skill tree
	public class SkillTree
	{
		// Variables
		List<HLabel> labels = new List<HLabel>(4);
		List<HSquare> squares = new List<HSquare>(15);
		List<HSquare> subsquares = new List<HSquare>(16);
		List<HLabel> sublabels = new List<HLabel>(16);
		List<HButton> buttons = new List<HButton>(2);

		string[] skillLabels = new string[15];
		List<SkillManager.Skill> skillData = new List<SkillManager.Skill>(15);
		float purchaseSkillTimer = 0f;

		Texture2D levelBorderPanelTexture, levelPanelTexture;
		Rectangle levelBorderPanelRectangle, levelPanelRectangle;

		// Methods
		/// Constructor
		public SkillTree()
		{
			// Resizes the window
			HeadWars.Resize(650, 430);

			// Title
			labels.Add(new HLabel(Translation.GetString("skill_tree"), Font.WindowTitle));
			labels[0].setPosition(0, 0, "xcenter top");

			// Points
			labels.Add(new HLabel(string.Format("{0}: {1:###}", Translation.GetString("points"), Info.levelPoint.ToString()), Font.BigTextBold));
			labels[1].setPosition(0, -50, "xcenter bottom");
			labels[1].textColor(Color.DeepSkyBlue);

			// Skills
			foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
			{
				skillLabels[skill.ID] = skill.CurrentStage + "/" + skill.Stages;
				skillData.Add(skill);
			}
			summonTriangle(5, (int)HeadWars.ScreenDimension.X / 2, 100, 32);

			// Redistribute
			buttons.Add(new HButton(Translation.GetString("redistribute")));
			buttons[0].setPosition(-60, 0, "right bottom");

			sublabels.Add(new HLabel(Translation.GetString("redistribute_cost"), Font.Text));
			sublabels[15].textColor(Color.Yellow);
			subsquares.Add(new HSquare(0, 0, (int)sublabels[15].measureString().X, (int)sublabels[15].measureString().Y));

			// Back to Menu
			buttons.Add(new HButton(Translation.GetString("button_back")));
			buttons[1].setPosition(60, 0, "left bottom");

			// Level bar
			int total = (100 * (Info.totalExperience - SkillManager.levelToExp(Info.level))) / (SkillManager.levelToExp(Info.level + 1) - SkillManager.levelToExp(Info.level)) * 2;

			levelPanelTexture = new Texture2D(HeadWars.Instance.GraphicsDevice, 204, 16);
			levelPanelRectangle = new Rectangle((int)HeadWars.ScreenDimension.X / 2 - 98 + total, (int)HeadWars.ScreenDimension.Y - 38, 196 - total, 16);

			levelBorderPanelTexture = new Texture2D(HeadWars.Instance.GraphicsDevice, 200, 20);
			levelBorderPanelRectangle = new Rectangle((int)HeadWars.ScreenDimension.X / 2 - 100, (int)HeadWars.ScreenDimension.Y - 40, 200, 20);

			levelPanelTexture.Fill(levelPanelTexture.Width * levelPanelTexture.Height);
			levelBorderPanelTexture.Fill(levelBorderPanelRectangle.Width * levelBorderPanelRectangle.Height);

			labels.Add(new HLabel(Translation.GetString("level") + " " + Info.level, Font.SmallTextBold));
			labels[2].setPosition(0, (int)HeadWars.ScreenDimension.Y - 55, "xcenter");
			labels.Add(new HLabel(levelBorderPanelRectangle, (Info.totalExperience - SkillManager.levelToExp(Info.level)) + "/" + (SkillManager.levelToExp(Info.level + 1) - SkillManager.levelToExp(Info.level)), Font.Text));
			labels[3].textColor(Color.White);
			labels[3].Name = "ExperienceBar";
		}

		/// Skills Triangle
		private void summonTriangle(int rows, int xCenter, int y, int width)
		{
			int currentSquare = 0;

			int xSpace = width + 16;

			for (int line = 0; line < rows; line++)
			{
				int x = xCenter - ((rows * 2) * xSpace / 2) + xSpace * (rows - line);
				for (int i = 0; i <= line; i++)
				{
					int currentX = x + (xSpace * i) + (line * xSpace / 2);
					squares.Add(new HSquare(Graphic.skills[currentSquare], currentX - width / 2, y, width, width));
					squares[currentSquare].setColor((skillData[currentSquare].CurrentStage >= skillData[currentSquare].Stages ? Color.Gold.Collection() : Color.DeepSkyBlue.Collection()));
					squares[currentSquare].setHoverSound = Sound.skillHover;

					sublabels.Add(new HLabel(Translation.GetString(skillData[currentSquare].Name) + " (" + skillLabels[currentSquare] + ")\n" + Translation.GetString(skillData[currentSquare].Description), Font.Text));
					sublabels[currentSquare].Name = currentSquare.ToString();
					subsquares.Add(new HSquare(0, 0, (int)sublabels[currentSquare].measureString().X, (int)sublabels[currentSquare].measureString().Y));

					currentSquare++;
				}
				y += xSpace + 2;
			}
		}

		/// Set blocked skills
		private void blockSkills(int rows)
		{
			int currentSquare = 0, rowPoints = 0;

			// Gets the total points
			for (int line = 0; line < rows; line++)
				for (int i = 0; i <= line; i++)
					rowPoints += skillData[currentSquare++].CurrentStage;

			currentSquare = 0;

			// At least 5 points per row
			for (int line = 0; line < rows; line++)
				for (int i = 0; i <= line; i++)
				{
					Boolean isLocked = rowPoints < (5 - (line + 1)) * 5;
					squares[currentSquare].opacity = isLocked ? .4f : 1f;
					squares[currentSquare].setContent(isLocked ? Graphic.lockedSkill : Graphic.skills[currentSquare]);
					currentSquare++;
				}
		}

		/// Spawn Popups
		private void showPopUp(int id, SpriteBatch ForegroundLayer, int Y = 60)
		{
			float txtSize = sublabels[id].measureString().X / 2;
			Vector2 coord = new Vector2(Control.MouseCoordinates.X - txtSize, Control.MouseCoordinates.Y - Y);
			if (coord.X <= 0)
				coord.X = 0;
			else if (coord.X + txtSize * 2 >= HeadWars.ScreenDimension.X)
				coord.X = HeadWars.ScreenDimension.X - txtSize * 2;

			subsquares[id].setPosition((int)coord.X, (int)coord.Y);
			subsquares[id].Draw(ForegroundLayer);

			sublabels[id].setPosition((int)coord.X, (int)coord.Y);

			string oldText = String.Empty;
			if (id < skillLabels.Length && squares[id].GetContent == Graphic.lockedSkill)
			{
				oldText = sublabels[id].text;
				sublabels[id].text = "??? (?/?)\n???";
			}

			sublabels[id].Draw(ForegroundLayer);
			if (oldText != String.Empty)
				sublabels[id].text = oldText;
		}

		/// Update
		public void Update()
		{
			foreach (HButton b in buttons)
				b.Update();
			foreach (HSquare s in squares)
				s.Update();
			foreach (HLabel l in labels)
				l.Update();

			if (buttons[0].onClick)
			{
				if (Info.levelPoint > 4)
				{
					Boolean hasSkill = false;
					DatabaseManager.Connect();
					hasSkill = DatabaseManager.hasSkills(HeadWars.playerName);

					if (hasSkill)
					{
						Info.levelPoint -= 5;

						if (SkillManager.skillData["trickTreat"].isActive)
						{
							foreach (Enemy.Info e in Enemy.enemyInfo.Values)
								e.Reset();

							BlackHole.standLife /= 2;
							BlackHole.standScorePoint /= 2;
						}

						skillData.Clear();
						foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
						{
							if (skill.isActive)
								Info.levelPoint += skill.CurrentStage;
							skill.Reset();

							skillLabels[skill.ID] = skill.CurrentStage + "/" + skill.Stages;
							skillData.Add(skill);
						}

						DatabaseManager.alterPlayerData(HeadWars.playerName, "points", Info.levelPoint);
						DatabaseManager.destroySkillTree(HeadWars.playerName);

						labels[1].text = string.Format("{0}: {1:###}", Translation.GetString("points"), Info.levelPoint.ToString());

						foreach (HSquare s in squares)
							s.setColor(Color.DeepSkyBlue.Collection());
						foreach (HLabel l in sublabels)
							if (l.Name != "")
								l.text = Strings.gsub(l.text, @"\(\d+/\d+\)", "(" + skillLabels[Convert.ToInt32(l.Name)] + ")");

						Sound.redistributeSkills.Play(.4f, Maths.random.RandomRange(-.2f, .2f), 0);
					}

					DatabaseManager.Disconnect();
				}
			}
			else if (buttons[1].onClick)
			{
				HeadWars.currentGameState = HeadWars.gameState.Menu;
				return;
			}

			int i = 0;
			foreach (HSquare s in squares)
			{
				if (Info.levelPoint > 0 && s.opacity == 1f && purchaseSkillTimer-- <= 0 && s.onClick)
				{
					foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
						if (skill.ID == i)
							if (skill.CurrentStage < skill.Stages)
							{
								purchaseSkillTimer = 200f;

								labels[1].text = string.Format("{0}: {1:###}", Translation.GetString("points"), (--Info.levelPoint).ToString());

								sublabels[i].text = Translation.GetString(skillData[i].Name) + " (" + ++skill.CurrentStage + "/" + skill.Stages + ")\n" + Translation.GetString(skillData[i].Description);
								typeof(SkillManager).GetMethod(skill.Function).Invoke(null, new object[] { });

								if (skill.CurrentStage >= skill.Stages)
									s.setColor(Color.Gold.Collection());

								DatabaseManager.Connect();
								DatabaseManager.alterPlayerData(HeadWars.playerName, "points", Info.levelPoint);

								if (skill.CurrentStage == 1)
									DatabaseManager.newPlayerSkill(HeadWars.playerName, skill.Function);
								else
									DatabaseManager.alterPlayerData(HeadWars.playerName, "skillStage", skill.CurrentStage, "PlayerSkillH", "and skill='" + skill.Function + "'");
								DatabaseManager.Disconnect();

								Sound.newSkill.Play(.4f, Maths.random.RandomRange(-.2f, .2f), 0);
							}
					break;
				}
				i++;
			}

			blockSkills(5);
		}

		/// Draw
		public void Draw(SpriteBatch BackgroundLayer, SpriteBatch MediumLayer, SpriteBatch ForegroundLayer)
		{
			foreach (HButton b in buttons)
				b.Draw(BackgroundLayer, MediumLayer);
			foreach (HSquare s in squares)
				s.Draw(BackgroundLayer);
			foreach (HLabel l in labels)
				l.Draw(l.Name == "ExperienceBar" ? MediumLayer : BackgroundLayer);

			int i = 0;
			foreach (HSquare s in squares)
			{
				if (s.MouseHover)
					// Skill description popup
					foreach (SkillManager.Skill skill in SkillManager.skillData.Values)
						if (skill.ID == i)
							showPopUp(i, ForegroundLayer);
				i++;
			}

			if (buttons[0].MouseHover)
				showPopUp(15, ForegroundLayer, 40);

			BackgroundLayer.Draw(levelBorderPanelTexture, levelBorderPanelRectangle, Color.CadetBlue);
			BackgroundLayer.Draw(levelPanelTexture, levelPanelRectangle, Color.DarkSlateGray);
		}
	}
}